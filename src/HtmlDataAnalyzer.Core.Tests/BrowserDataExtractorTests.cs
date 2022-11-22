namespace HtmlDataAnalyzer.Core.Tests
{
    public class BrowserDataExtractorTests
    {
        [Fact]
        public async Task InitializeBrowser()
        {
            var browserDataExtractor = new BrowserDataExtractor();
            await browserDataExtractor.InitializeBrowser();
        }

        [Theory]
        [InlineData("https://google.com")]
        [InlineData("https://facebook.com")]
        public async Task ExecuteAnalyzingAsync(string url)
        {
            var browserDataExtractor = new BrowserDataExtractor();
            var result = await browserDataExtractor.ExecuteAnalyzingAsync(url);

            Assert.NotNull(result);
        }
    }
}
