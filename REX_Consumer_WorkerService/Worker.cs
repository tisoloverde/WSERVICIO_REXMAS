using Microsoft.Extensions.Options;
using REX_Consumer_WorkerService.FlujoActualiza;
using REX_Consumer_WorkerService.FlujoCarga;
using REX_Consumer_WorkerService.Models;

namespace REX_Consumer_WorkerService
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private IConfiguration config;
		private Resultado resultado = new Resultado();

		public Worker(ILogger<Worker> logger, IConfiguration configuration)
		{
			_logger = logger;
			config= configuration;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

					CargaDesdeRex cargaDesdeRex = new CargaDesdeRex(config);
					resultado = await  cargaDesdeRex.CargarDatos();

					ActualizaDatos actualizaDatos = new ActualizaDatos(config);
					resultado = await  actualizaDatos.ActualizaDatosBD();


				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "An error occurred while calling the API extern.");
				}

				await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
			}
		}
	}
}
