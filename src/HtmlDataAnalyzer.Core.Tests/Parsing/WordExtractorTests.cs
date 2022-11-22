using HtmlDataAnalyzer.Core.Parsing;
using System.Text.RegularExpressions;

namespace HtmlDataAnalyzer.Core.Tests.Parsing
{
    public class WordExtractorTests
    {
        public static IEnumerable<object[]> ExecuteSiteAsyncData =>
            new List<object[]>
            {
                new object[] { Array.Empty<string>(), new Dictionary<string, int>() },
                new object[] { new[] { "one" }, new Dictionary<string, int> { { "one", 1 } } },
                new object[] { new[] { "five,five;five. five\n;\t\t    five" }, new Dictionary<string, int> { { "five", 5 } } },
                new object[] { new[] { "español" }, new Dictionary<string, int> { { "español", 1 } } },
                new object[] { new[] { "беларуская мова" }, new Dictionary<string, int> { { "беларуская", 1 }, { "мова", 1 } } },
            };

        [Theory]
        [MemberData(nameof(ExecuteSiteAsyncData))]
        public Task ExecuteSiteAsync_InputScenario_ExpectedResult(
            string[] inputLines,
            IDictionary<string, int> exceptedResult)
        {
            var actualResult = WordExtractor.ExtractWordsCountFromLines(inputLines);

            Assert.Equal(exceptedResult, actualResult);
            return Task.CompletedTask;
        }
    }
}
