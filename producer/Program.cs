using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        var currentDirectory = hostingContext.HostingEnvironment.ContentRootPath;
        config.SetBasePath(currentDirectory)
            .AddJsonFile("settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
        config.Build();
    })
    .ConfigureServices((ctx, svc) =>
    {
        var producerConfig = new ProducerConfig();
        ctx.Configuration.GetSection("ConfluentCloud").Bind(producerConfig);
        svc.AddSingleton<ProducerConfig>(producerConfig);
    })
    .Build();

host.Run();
