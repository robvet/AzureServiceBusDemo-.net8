using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ServiceBusSqlAdapter;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    // Add user secrets
    .ConfigureAppConfiguration(c =>
    {
        c.AddUserSecrets<ClaimsProcessing>();
    })
    .Build();

host.Run();
