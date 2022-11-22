using PuppeteerSharp;
using System.Diagnostics;
using System.Reflection;

namespace HtmlDataAnalyzer.Core.Tests
{
    internal class TestUtils
    {
        public static async Task<IBrowser> ResolveBrowser()
        {
            using var browserFetcher = new BrowserFetcher();

            await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            return browser;
        }

        public static async Task<string> LoadContentFromResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            await using var stream = assembly.GetManifestResourceStream(resourceName);
            Debug.Assert(stream != null, nameof(stream) + " != null");

            using var reader = new StreamReader(stream);
            var content = await reader.ReadToEndAsync();
            return content;
        }
    }
}
