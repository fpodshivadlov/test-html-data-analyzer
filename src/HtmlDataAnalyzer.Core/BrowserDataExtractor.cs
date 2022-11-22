using HtmlDataAnalyzer.Core.Model;
using PuppeteerSharp;
using HtmlDataAnalyzer.Core.Parsing;

namespace HtmlDataAnalyzer.Core
{
    public class BrowserDataExtractor
    {
        public async Task InitializeBrowser()
        {
            using var browserFetcher = new BrowserFetcher();

            await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
        }

        public async Task<PageContentResultsModel> ExecuteAnalyzingAsync(
            string url,
            TimeSpan? loadDelay = null)
        {
            await InitializeBrowser();
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            await page.GoToAsync(url);

            await Task.Delay(loadDelay ?? TimeSpan.Zero);

            var result = await PageAnalyzer.AnalyzePageAsync(page);

            return result;
        }
    }
}
