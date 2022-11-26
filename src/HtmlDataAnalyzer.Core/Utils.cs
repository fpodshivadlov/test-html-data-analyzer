using PuppeteerSharp;

namespace HtmlDataAnalyzer.Core
{
    public class Utils
    {
        public static LaunchOptions CreateLaunchOptions()
        {
            return new LaunchOptions
            {
                Headless = true,
                Args = new[]
                {
                    "--disable-web-security",
                    "--enable-usermedia-screen-capturing",
                    "--allow-http-screen-capture",
                    "--disable-features=IsolateOrigins,site-per-process"
                },
            };
        }
    }
}
