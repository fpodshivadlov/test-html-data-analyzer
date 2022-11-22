using PuppeteerSharp;
using HtmlDataAnalyzer.Core.Parsing;

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

            var result = await PageAnalyzer.AnalyzePageAsync(page);

            return page;
        }
    }
}
