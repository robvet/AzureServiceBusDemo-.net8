using AzureServiceBus.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

// Bind Azure Service Bus settings
var azureServiceBusConfig = new ServiceBusConfig();
builder.Configuration.GetSection("AzureServiceBus").Bind(azureServiceBusConfig);
builder.Services.AddSingleton(azureServiceBusConfig);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(logging =>
{
    logging.AddApplicationInsights(builder.Configuration["ApplicationInsights:InstrumentationKey"]);
    logging.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information);
});

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
