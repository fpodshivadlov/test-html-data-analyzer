namespace HtmlDataAnalyzer.Core
{
    public class ImageDataModel
    {
        public string? Url { get; init; }

        public string? Name { get; init; }
    }

    public class PageContentResultsModel
    {
        public ImageDataModel[]? Images { get; init; }

        public IDictionary<string, uint>? Keywords { get; init; }
    }
}
