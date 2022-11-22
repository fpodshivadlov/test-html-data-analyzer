using System.Text.RegularExpressions;

namespace HtmlDataAnalyzer.Core.Parsing
{
    public class WordExtractor
    {
        public static IDictionary<string, int> ExtractWordsCountFromLines(IEnumerable<string> filteredContentItems)
        {
            IEnumerable<string> SplitToWords(string contentItem)
            {
                const string regexPattern = @"^((?<word>[\w'-]+)[^\w'-]*)*$";
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

            var wordCounts = filteredContentItems
                .SelectMany(SplitToWords)
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, y => y.Count());

            return wordCounts;
        }
    }
}
