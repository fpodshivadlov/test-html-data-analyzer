using HtmlDataAnalyzer.Core.Parsing;

namespace HtmlDataAnalyzer.Core.Tests.Parsing
{
    public class PageAnalyzerTests
    {
        private const string ResourceNameBasePath = "HtmlDataAnalyzer.Core.Tests.Resources";
        private const string WordsResourceName = $"{ResourceNameBasePath}.Words.html";
        private const string ImagesResourceName = $"{ResourceNameBasePath}.Images.html";

        [Fact]
        public async Task ExecuteSite_WordsResource_ValidWordsAnalysis()
        {
            var browser = await TestUtils.ResolveBrowser();
            var page = await browser.NewPageAsync();

            var content = await TestUtils.LoadContentFromResource(WordsResourceName);
            await page.SetContentAsync(content);

            var result = await PageAnalyzer.AnalyzePageAsync(page);
            Assert.NotNull(result);

            Assert.NotNull(result.Words);
            Assert.Contains("Visible", result.Words.Keys);
            Assert.DoesNotContain("Hidden", result.Words.Keys);
        }

        [Fact]
        public async Task ExecuteSite_ImagesResource_ValidWordsAnalysis()
        {
            var browser = await TestUtils.ResolveBrowser();
            var page = await browser.NewPageAsync();

            var content = await TestUtils.LoadContentFromResource(ImagesResourceName);
            await page.SetContentAsync(content);

            var result = await PageAnalyzer.AnalyzePageAsync(page);
            Assert.NotNull(result);

            Assert.NotNull(result.Images);
            Assert.Contains(result.Images, x => x.Name?.Contains("Visible") ?? false);
            Assert.Equal(2, result.Images.Count);

            Assert.Contains(result.Images, x => x.PngImage != null);
        }
    }
}
