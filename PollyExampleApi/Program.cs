using PollyExampleApi.Policies;
using PollyExampleApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHttpClient<IService, Service>()
    .AddPolicyHandler(RetryPolicy.GetGenericRetryPolicy(retryCount: 3))
    .AddPolicyHandler(CircuitBreakerPolicy.GetCircuitBreakerPolicy(exceptionsAllowedBeforeBreaking: 5, durationOfBreakInSeconds: 30));

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
