using PuppeteerSharp;
using System.Text.RegularExpressions;
using HtmlDataAnalyzer.Core.JavaScriptWrapping;

namespace HtmlDataAnalyzer.Core
{
    public class BrowserDataExtractor
    {
        public BrowserDataExtractor()
        {
        }

        public async Task<IPage> ExecuteSiteAsync(
            string url,
            TimeSpan? loadDelay = null)
        {
            using var browserFetcher = new BrowserFetcher();

            await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            await page.GoToAsync(url);

            await Task.Delay(loadDelay ?? TimeSpan.Zero);

            // ToDo: use streams 
            await page.ScreenshotAsync(
                "test-result.png",
                new ScreenshotOptions { Type = ScreenshotType.Png, FullPage = true });

            var words = await ResolveTextElements(page);

            return page;
        }

        private static async Task<ICollection<string>> ResolveTextElements(IPage page)
        {
            // ToDo: improve the text selection. Now input text is not returned.
            var textElementHandles = await page.XPathAsync("//text()");

            var filteredContentItems = new List<string>();
            foreach (var elementHandle in textElementHandles)
            {
                var boundingBox = await elementHandle.BoundingBoxAsync();
                if (boundingBox is {Width: > 0, Height: > 0})
                {
                    var elementData = await page.EvaluateFunctionAsync<ElementData>(
                        ElementData.Expression,
                        elementHandle);
                    if (!string.IsNullOrEmpty(elementData.Content))
                    {
                        filteredContentItems.Add(elementData.Content);
                    }
                }
            }

            // ToDo: extract to separate method and cover with unit tests
            IEnumerable<string> SplitToWords(string contentItem)
            {
                const string regexPattern = @"^((?<word>\S+)\s*)+$";
                foreach (Match match in Regex.Matches(contentItem, regexPattern))
                {
                    if (match.Groups.TryGetValue("word", out var group))
                    {
                        foreach (Capture capture in group.Captures)
                        {
                            yield return capture.Value;
                        }
                    }
                }
            }

            var words = filteredContentItems
                .SelectMany(SplitToWords)
                .ToList();

            return words;
        }
    }
}
