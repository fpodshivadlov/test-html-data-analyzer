namespace HtmlDataAnalyzer.Core.JavaScriptWrapping
{
    internal class BackgroundImageElement
    {
        public const string Expression = @"e => ({
            innerText: e.innerText,
            base64png: (" + Functions.GetBase64PngImage + @")((" + Functions.GetImgTag + @")(e)),
            base64png2: (" + Functions.GetImgTag + @")(e),
        })";

        public string? InnerText { get; set; }

        public string? Base64Png { get; set; }
    }
}
