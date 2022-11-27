namespace HtmlDataAnalyzer.Core.JavaScriptWrapping
{
    public class Functions
    {
        internal const string GetBase64PngImage = @"
function (img) {
    if (!img)
        return null;

    var canvas = document.createElement('canvas');
    canvas.width = img.width;
    canvas.height = img.height;
    var ctx = canvas.getContext('2d');
    ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
    var dataUrl = canvas.toDataURL('image/png');
    var contentMatch = dataUrl.match(/^data:image\/png;base64,(.*)$/);
    return contentMatch ? contentMatch[1] : null;
}";

        internal const string GetImgTag = @"
function (element) {
    var propData = window.getComputedStyle(element, null).getPropertyValue('background-image');
    const srcChecker = /url\(\s*?['""]?\s*?(\S+?)\s*?[""']?\s*?\)/i;
    let dataMatch = srcChecker.exec(propData);

    if (dataMatch) {
        var img = document.createElement('img');
        img.src = dataMatch[1];
        return img;
    }

    return null;
}";
    }
}
