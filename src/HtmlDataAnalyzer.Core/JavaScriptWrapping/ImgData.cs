namespace HtmlDataAnalyzer.Core.JavaScriptWrapping
{
    internal class ImgData
    {
        public const string Expression = "e => ({ alt: e.alt, src: e.src })";

        public string? Alt { get; set; }

        public string? Src { get; set; }
    }
}
