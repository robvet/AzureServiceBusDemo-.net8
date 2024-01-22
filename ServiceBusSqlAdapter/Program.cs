using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceBusSqlAdapter;
using ServiceBusSqlAdapter.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    // Add user secrets
    .ConfigureAppConfiguration(c =>
    {
        c.AddUserSecrets<ClaimsProcessing>();
    })
    .ConfigureServices((hostContext, services) =>
    {
        Console.WriteLine(hostContext.Configuration.GetConnectionString("ServiceBusDemoDBConnection"));

        // The GetConnectionString method specifically looks for connection strings under the "ConnectionStrings"
        // section of your configuration. Ensure that your connection string is prefixed with
        // "ConnectionStrings": {
        //      "ServiceBusDemoDBConnection": "<the secrets>"
        // }     
        services.AddDbContext<ServiceBusDemoSqldbContext>(options =>
            options.UseSqlServer(hostContext.Configuration.GetConnectionString("ServiceBusDemoDBConnection")));
        // Add Application Insights logging
        services.AddLogging(logging =>
        {
            logging.AddApplicationInsights(hostContext.Configuration["ApplicationInsights:InstrumentationKey"]);
            logging.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information);
        });
    })
    .Build();

host.Run();
