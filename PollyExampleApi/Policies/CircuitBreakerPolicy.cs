using Polly;
using Polly.Extensions.Http;

namespace PollyExampleApi.Policies
{
    public class CircuitBreakerPolicy
    {
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(int exceptionsAllowedBeforeBreaking, int durationOfBreakInSeconds)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking,
                    TimeSpan.FromSeconds(durationOfBreakInSeconds),
                    onBreak: (_, time) =>
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Circuito aberto sem permitir requisições durante {time}");
                    },
                    onReset: () =>
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Circuito fechado. Permitindo requisições.");
                    },
                    onHalfOpen: () =>
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Circuito semi aberto. Testando a próxima requisição.");
                    });
        }
    }
}
