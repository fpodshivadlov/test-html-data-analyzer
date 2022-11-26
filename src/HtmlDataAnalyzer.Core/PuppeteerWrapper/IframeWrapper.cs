using PuppeteerSharp;

namespace HtmlDataAnalyzer.Core.PuppeteerWrapper;

internal class IframeWrapper : IRootWrapper
{
    private readonly IFrame inner;

    public IframeWrapper(IFrame inner)
    {
        this.inner = inner;
    }

    public Task<IElementHandle[]> XPathAsync(string expression)
    {
        return inner.XPathAsync(expression);
    }

    public Task<T?> EvaluateFunctionAsync<T>(string script, params object[] args)
    {
        return inner.EvaluateFunctionAsync<T>(script, args);
    }
}