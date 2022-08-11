using PollyExampleApi.Policies;
using PollyExampleApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//FallbackPolicy fallbackPolicy = new();

builder.Services.AddHttpClient<IService, Service>()
    .AddPolicyHandler(RetryPolicy.GetGenericRetryPolicy(retryCount: 3))
    //.AddPolicyHandler(fallbackPolicy.GetFallback()) // A ordem das policies interferem na execução pois nesse caso o fallback é em cima da exceção do circuit braker
    .AddPolicyHandler(CircuitBreakerPolicy.GetCircuitBreakerPolicy(exceptionsAllowedBeforeBreaking: 5, durationOfBreakInSeconds: 15));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
