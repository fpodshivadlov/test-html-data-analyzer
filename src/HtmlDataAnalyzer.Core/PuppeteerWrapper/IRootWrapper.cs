using PuppeteerSharp;

namespace HtmlDataAnalyzer.Core.PuppeteerWrapper
{
    internal interface IRootWrapper
    {
        Task<IElementHandle[]> XPathAsync(string expression);

        Task<T?> EvaluateFunctionAsync<T>(string script, params object[] args);
    }
}
