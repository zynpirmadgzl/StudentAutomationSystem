using Microsoft.JSInterop;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAutomationSystem.Web.Services.Handlers
{
    public class AuthMessageHandler : DelegatingHandler
    {
        private readonly IJSRuntime _js;

        public AuthMessageHandler(IJSRuntime js)
        {
            _js = js;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
