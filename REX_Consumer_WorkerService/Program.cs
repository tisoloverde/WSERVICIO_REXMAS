using REX_Consumer_WorkerService;
using REX_Consumer_WorkerService.Models;

var host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((hostContext, services) =>
	{
		// Registrar la configuración de "MiConfiguracion"
		services.Configure<MiConfiguracion>(hostContext.Configuration.GetSection("MiConfiguracion"));

		// Registrar el servicio del Worker
		services.AddHostedService<Worker>();
	})
	.Build();

await host.RunAsync();
