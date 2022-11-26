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

            await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision)
                .ConfigureAwait(false);
        }

        public async Task<PageContentResultsModel> ExecuteAnalyzingAsync(
            string url,
            TimeSpan? loadDelay = null)
        {
            await InitializeBrowser()
                .ConfigureAwait(false);
            await using var browser = await Puppeteer.LaunchAsync(
                Utils.CreateLaunchOptions())
                .ConfigureAwait(false);

            var page = await browser.NewPageAsync();
            await page.GoToAsync(url, WaitUntilNavigation.Networkidle0)
                .ConfigureAwait(false);

            await Task.Delay(loadDelay ?? TimeSpan.Zero)
                .ConfigureAwait(false);

            var result = await PageAnalyzer.AnalyzePageAsync(page)
                .ConfigureAwait(false);

            return result;
        }
    }
}
