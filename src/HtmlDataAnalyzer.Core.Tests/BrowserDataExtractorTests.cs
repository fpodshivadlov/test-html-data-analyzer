namespace HtmlDataAnalyzer.Core.Tests
{
    public class BrowserDataExtractorTests
    {
        [Fact]
        public async Task ExecuteSite_Test()
        {
            var browserDataExtractor = new BrowserDataExtractor();
            var result = await browserDataExtractor.ExecuteSiteAsync("https://google.com");

            Assert.NotNull(result);
        }
    }
}
