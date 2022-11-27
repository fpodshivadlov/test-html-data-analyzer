using HtmlDataAnalyzer.Core.JavaScriptWrapping;
using HtmlDataAnalyzer.Core.Model;
using HtmlDataAnalyzer.Core.PuppeteerWrapper;
using PuppeteerSharp;

namespace HtmlDataAnalyzer.Core.Parsing
{
    public class PageAnalyzer
    {
        public static async Task<PageContentResultsModel> AnalyzePageAsync(
            IPage page)
        {
            var roots = await ResolveRoots(page)
                .ConfigureAwait(false);

            var words = await ResolveTextElements(roots)
                .ConfigureAwait(false);
            var images = await ResolveImages(roots)
                .ConfigureAwait(false);

            var result = new PageContentResultsModel
            {
                Words = words,
                Images = images,
            };

            return result;
        }

        private static async Task<ICollection<IRootWrapper>> ResolveRoots(IPage page)
        {
            var result = new List<IRootWrapper>
            {
                new PageWrapper(page),
            };

            var iframesElements = await page.XPathAsync("//iframe")
                .ConfigureAwait(false);

            foreach (var iframeElement in iframesElements)
            {
                var iframe = await iframeElement.ContentFrameAsync()
                    .ConfigureAwait(false);

                result.Add(new IframeWrapper(iframe));
            }

            return result;
        }

        private static async Task<IDictionary<string, int>> ResolveTextElements(
            IEnumerable<IRootWrapper> roots)
        {
            var filteredContentItems = new List<string>();

            foreach (var root in roots)
            {
                var elementHandles = await root.XPathAsync("//text()")
                    .ConfigureAwait(false);

                foreach (var elementHandle in elementHandles)
                {
                    var isVisible = await ResolveVisibility(elementHandle)
                        .ConfigureAwait(false);
                    if (isVisible)
                    {
                        var elementData = await root.EvaluateFunctionAsync<ElementContent>(
                            ElementContent.Expression,
                            elementHandle)
                            .ConfigureAwait(false);

                        if (!string.IsNullOrEmpty(elementData?.Content))
                        {
                            filteredContentItems.Add(elementData.Content);
                        }
                    }
                }
            }

            var words = WordExtractor.ExtractWordsCountFromLines(filteredContentItems);

            return words;
        }

        private static async Task<ICollection<ImageDataModel>> ResolveImages(
            IEnumerable<IRootWrapper> roots)
        {
            var result = new List<ImageDataModel>();

            foreach (var root in roots)
            {
                var imgElementHandles = await root.XPathAsync("//img")
                    .ConfigureAwait(false);

                foreach (var imgElementHandle in imgElementHandles)
                {
                    var isVisible = await ResolveVisibility(imgElementHandle, true)
                        .ConfigureAwait(false);
                    if (isVisible)
                    {
                        var data = await root.EvaluateFunctionAsync<ImgData>(
                                ImgData.Expression,
                                imgElementHandle)
                            .ConfigureAwait(false);

                        if (data?.Base64Png != null && data.Visible == true)
                        {
                            result.Add(new ImageDataModel
                            {
                                Name = data.Alt,
                                PngImage = Convert.FromBase64String(data.Base64Png),
                            });
                        }
                    }
                }

                var elementHandles = await root.XPathAsync("//*")
                    .ConfigureAwait(false);

                foreach (var elementHandle in elementHandles)
                {
                    var isVisible = await ResolveVisibility(elementHandle)
                        .ConfigureAwait(false);
                    if (isVisible)
                    {
                        var data = await root.EvaluateFunctionAsync<BackgroundImageElement>(
                                BackgroundImageElement.Expression,
                                elementHandle)
                            .ConfigureAwait(false);

                        if (data?.Base64Png != null)
                        {
                            result.Add(new ImageDataModel
                            {
                                Name = data.InnerText,
                                PngImage = Convert.FromBase64String(data.Base64Png),
                            });
                        }
                    }
                }
            }

            return result;
        }

        private static async Task<bool> ResolveVisibility(
            IElementHandle elementHandle, bool visibilityFallback = false)
        {
            var a = await elementHandle.BoxModelAsync()
                .ConfigureAwait(false);
            var boundingBox = await elementHandle.BoundingBoxAsync()
                .ConfigureAwait(false);

            // If we cannot determine visibility, consider `visibilityFallback`
            // ToDo: make configurable
            if (boundingBox == null)
                return visibilityFallback;

            var isVisible = boundingBox is { Width: > 0, Height: > 0 };
            return isVisible;
        }
    }
}
