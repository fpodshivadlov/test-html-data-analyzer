﻿namespace HtmlDataAnalyzer.Core.JavaScriptWrapping
{
    internal class ImgData
    {
        public const string Expression = @"e => ({
            alt: e.alt,
            base64png: (" + Functions.GetBase64PngImage + @")(e),
            visible: (" + Functions.GetVisibility + @")(e),
        })";

        public string? Alt { get; set; }

        public string? Base64Png { get; set; }

        public bool? Visible { get; set; }
    }
}
