using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System.Net;

namespace PollyExampleApi.Policies
{
    public class RetryPolicy
    {
        public static IAsyncPolicy<HttpResponseMessage> GetGenericRetryPolicy(int retryCount)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                //Calcula a duração de espera entre as tentativas de forma exponencial
                // 2^1 = 2 segundos
                // 2^2 = 4 segundos
                // 2^3 = 8 segundos
                // .....
                .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public static IAsyncPolicy<HttpResponseMessage> GetToManyRequestsRetryPolicy(int retryCount)
        {
            return Policy.Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(ex => ex.StatusCode == HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public static IAsyncPolicy<HttpResponseMessage> GetServiceUnavailableRetryPolicy(int retryCount)
        {
            return Policy.Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(ex => ex.StatusCode == HttpStatusCode.ServiceUnavailable)
                .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
