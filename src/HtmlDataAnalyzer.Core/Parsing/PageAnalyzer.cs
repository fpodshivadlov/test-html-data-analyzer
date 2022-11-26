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
            var elementHandles = await page.XPathAsync("//text()");

            var filteredContentItems = new List<string>();
            foreach (var elementHandle in elementHandles)
            {
                var isVisible = await ResolveVisibility(elementHandle);
                if (isVisible)
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
            var elementHandles = await page.XPathAsync("//img");

            var result = new List<ImageDataModel>();
            foreach (var elementHandle in elementHandles)
            {
                var isVisible = await ResolveVisibility(elementHandle, true);
                if (isVisible)
                {
                    var data = await page.EvaluateFunctionAsync<ImgData>(
                        ImgData.Expression,
                        elementHandle);

                    if (data?.Base64Png != null)
                    {
                        result.Add(new ImageDataModel
                        {
                            Name = data.Alt,
                            PngImage = Convert.FromBase64String(data.Base64Png),
                        });
                    }
                }
            }

            return result;
        }

        private static async Task<bool> ResolveVisibility(
            IElementHandle elementHandle, bool visibilityFallback = false)
        {
            var boundingBox = await elementHandle.BoundingBoxAsync();

            // If we cannot determine visibility, consider `visibilityFallback`
            // ToDo: make configurable
            if (boundingBox == null)
                return visibilityFallback;

            var isVisible = boundingBox is { Width: > 0, Height: > 0 };
            return isVisible;
        }
    }
}
