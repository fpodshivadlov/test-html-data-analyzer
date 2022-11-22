namespace HtmlDataAnalyzer.Core.Model
{
    public class PageContentResultsModel
    {
        public ICollection<ImageDataModel>? Images { get; init; }

        public IDictionary<string, int>? Words { get; init; }
    }
}
