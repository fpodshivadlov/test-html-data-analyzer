using HtmlDataAnalyzer.Core.JavaScriptWrapping;
using HtmlDataAnalyzer.Core.Model;
using PuppeteerSharp;

namespace HtmlDataAnalyzer.Core.Parsing
{
    public class PageAnalyzer
    {
        public static async Task<PageContentResultsModel> AnalyzePageAsync(
            IPage page)
        {
            var words = await ResolveTextElements(page);
            var images = await ResolveImages(page);

            var result = new PageContentResultsModel
            {
                Words = words,
                Images = images,
            };

            return result;
        }

        private static async Task<IDictionary<string, int>> ResolveTextElements(IPage page)
        {
            var textElementHandles = await page.XPathAsync("//text()");

            var filteredContentItems = new List<string>();
            foreach (var elementHandle in textElementHandles)
            {
                if (await IsElementVisible(elementHandle))
                {
                    var elementData = await page.EvaluateFunctionAsync<ElementContent>(
                        ElementContent.Expression,
                        elementHandle);

                    if (!string.IsNullOrEmpty(elementData.Content))
                    {
                        filteredContentItems.Add(elementData.Content);
                    }
                }
            }

            var words = WordExtractor.ExtractWordsCountFromLines(filteredContentItems);

            return words;
        }

        private static async Task<ICollection<ImageDataModel>> ResolveImages(IPage page)
        {
            var textElementHandles = await page.XPathAsync("//img");

            var result = new List<ImageDataModel>();
            foreach (var elementHandle in textElementHandles)
            {
                if (await IsElementVisible(elementHandle))
                {
                    var data = await page.EvaluateFunctionAsync<ImgData>(
                        ImgData.Expression,
                        elementHandle);

                    if (!string.IsNullOrEmpty(data.Src))
                    {
                        result.Add(new ImageDataModel
                        {
                            Name = data.Alt,
                            Url = data.Src,
                        });
                    }
                }
            }

            return result;
        }

        private static async Task<bool> IsElementVisible(IElementHandle elementHandle)
        {
            var boundingBox = await elementHandle.BoundingBoxAsync();
            return boundingBox is { Width: > 0, Height: > 0 };
        }

    }
}
