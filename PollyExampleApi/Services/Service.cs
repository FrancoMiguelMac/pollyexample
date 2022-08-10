namespace PollyExampleApi.Services
{
    public class Service : IService
    {
        private readonly HttpClient _httpClient;
        public Service(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Get(int code)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://httpbin.org/status/{code}");
                Console.WriteLine(response.IsSuccessStatusCode);
            }
            catch (Polly.CircuitBreaker.BrokenCircuitException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Requisição bloqueada.");
            }
        }
    }
}
