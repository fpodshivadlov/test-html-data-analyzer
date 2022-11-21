namespace HtmlDataAnalyzer.Core.JavaScriptWrapping
{
    internal class ElementData
    {
        public const string Expression = "e => ({ content: e.textContent })";

        public string? Content { get; set; }
    }
}
