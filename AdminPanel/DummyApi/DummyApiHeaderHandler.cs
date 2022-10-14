using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AdminPanel.DummyApi;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace AdminPanel;

[UsedImplicitly]
public class DummyApiHeaderHandler : DelegatingHandler
{
    private readonly DummyApiOptions _options;

    public DummyApiHeaderHandler(IOptions<DummyApiOptions> options)
        => _options = options.Value;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("app-id", _options.AppId);
        return await base.SendAsync(request, cancellationToken);
    }
}
