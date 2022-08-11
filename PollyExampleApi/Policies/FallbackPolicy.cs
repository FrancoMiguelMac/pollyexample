using Polly;
using Polly.CircuitBreaker;
using System.Net;
using System.Text.Json;

namespace PollyExampleApi.Policies
{
    public class FallbackPolicy
    {
        public IAsyncPolicy<HttpResponseMessage> GetFallback()
        {
            return Policy<HttpResponseMessage>
                .Handle<BrokenCircuitException>()
                .FallbackAsync(FallbackAction, OnFallbackAsync);
        }

        private Task OnFallbackAsync(DelegateResult<HttpResponseMessage> response, Context context)
        {
            Console.WriteLine("Prestes a chamar a ação de fallback. Este é um bom lugar para fazer algum registro em log.");
            return Task.CompletedTask;
        }

        private Task<HttpResponseMessage> FallbackAction(DelegateResult<HttpResponseMessage> responseToFailedRequest, Context context, CancellationToken cancellationToken)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize((Nome: "Miguel Mac Franco", Idade: 34)))
            };
            return Task.FromResult(httpResponseMessage);
        }
    }
}
