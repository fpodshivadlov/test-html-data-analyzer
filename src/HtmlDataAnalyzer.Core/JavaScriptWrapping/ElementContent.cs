namespace HtmlDataAnalyzer.Core.JavaScriptWrapping
{
    internal class ElementContent
    {
        public const string Expression = "e => ({ content: e.textContent })";

        public string? Content { get; set; }
    }
}
