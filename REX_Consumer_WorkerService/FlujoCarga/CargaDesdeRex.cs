using REX_Consumer_WorkerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Xml.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Contracts;

namespace REX_Consumer_WorkerService.FlujoCarga
{
	public  class CargaDesdeRex
	{
		private readonly HttpClient _httpClient;
		private readonly string _connectionString="";
		private readonly MiConfiguracion _miConfiguracion;
		private Resultado resultado = new Resultado();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="configuration"></param>
		public CargaDesdeRex(IConfiguration configuration)
		{
			_httpClient = new HttpClient();
			_connectionString = configuration.GetConnectionString("DefaultConnection");

			_miConfiguracion = new MiConfiguracion
			{
				UsuarioLogin = configuration["MiConfiguracion:UsuarioLogin"],
				PasswordLogin = configuration["MiConfiguracion:PasswordLogin"],
				UrlBase = configuration["MiConfiguracion:UrlBase"],
				TodosLosContratos = configuration["MiConfiguracion:TodosLosContratos"],
				FechaActual = DateTime.Now,
				FechaDiaAnterior = DateTime.Now.AddDays(-1),
				AplicaConstantes = configuration["MiConfiguracion:AplicaConstantes"],
				FechaCorteColaborador = configuration["MiConfiguracion:FechaCorteColaborador"],
				FechaInicioVacacion = configuration["MiConfiguracion:FechaInicioVacacion"],
				FechaInicioLicenciaMedica = configuration["MiConfiguracion:FechaInicioLicenciaMedica"],
				FechaInicioPermisos = configuration["MiConfiguracion:FechaInicioPermisos"],
				CatalogoBanco = configuration["MiConfiguracion:CatalogoBanco"],
				CatalogoProfesion = configuration["MiConfiguracion:CatalogoProfesion"],
				CatalogoFormaPago = configuration["MiConfiguracion:CatalogoFormaPago"],
				CatalogoCentroCosto = configuration["MiConfiguracion:CatalogoCentroCosto"],
				CatalogoCargo = configuration["MiConfiguracion:CatalogoCargo"],
				CatalogoLicenciaConducir = configuration["MiConfiguracion:CatalogoLicenciaConducir"],
				CatalogoNivelEstudio = configuration["MiConfiguracion:CatalogoNivelEstudio"],
				CatalogoNivelOcupacional = configuration["MiConfiguracion:CatalogoNivelOcupacional"],
				CatalogoRelaciones = configuration["MiConfiguracion:CatalogoRelaciones"],
				CatalogoCausalesTerminoContrato = configuration["MiConfiguracion:CatalogoCausalesTerminoContrato"],

				CatalogoCargoGenericoUnificado = configuration["MiConfiguracion:CatalogoCargoGenericoUnificado"],
				CatalogoReferencia1 = configuration["MiConfiguracion:CatalogoReferencia1"],
				CatalogoReferencia2 = configuration["MiConfiguracion:CatalogoReferencia2"],
				CatalogoFeriados = configuration["MiConfiguracion:CatalogoFeriados"],
				CatalogoFeriadosMovi = configuration["MiConfiguracion:CatalogoFeriadosMovi"],

				ConstanteTipoContrato = configuration["MiConfiguracion:ConstanteTipoContrato"],
				ConstanteEstadoContrato = configuration["MiConfiguracion:ConstanteEstadoContrato"],


			};

		}

		public async Task<Resultado> CargarDatos()
		{
			string contents;
			// Logic to load data from REX
			// Obtenemos el token de acceso desde la API de REX
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Put, _miConfiguracion.UrlBase + "/api/v2/aliados/configuracion/recuperar_token");
			var content = new StringContent("{\"usuario\":\"" + _miConfiguracion.UsuarioLogin + "\",\"clave\":\"" + _miConfiguracion.PasswordLogin + "\"}", null, "application/json");
			request.Content = content;


			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();
			var loginToken = JsonConvert.DeserializeObject<LoginToken>(contents);

			if(loginToken == null)
			{
				Console.WriteLine("Error al obtener el token de acceso.");
				return resultado; 
			}else if(loginToken.Token == null)
			{
				Console.WriteLine("Error al obtener el token de acceso.");
				return resultado; 
			}
			else
			{

				// Parametrias generales / Catalogos
				if (_miConfiguracion.AplicaConstantes.Trim().Equals("S"))
				{
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine("Inicio carga de datos desde REX...");
					// Carga referencia empresas
					resultado = await CargaEmpresas(loginToken.Token);
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine("Inicio carga de datos desde REX...");
					// Carga referencia bancos
					resultado = await CargaBancos(loginToken.Token);
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine("Inicio carga de datos desde REX...");
					// Carga referencia profesiones
					resultado = await CargaProfesiones(loginToken.Token);
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine("Inicio carga de datos desde REX...");
					// Carga referencia formas de pago
					resultado = await CargaFormasPago(loginToken.Token);
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine("Inicio carga de datos desde REX...");
					// Carga referencia formas de pago
					resultado = await CargaLicenciasConducir(loginToken.Token);
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine("Inicio carga de datos desde REX...");
					// Carga referencia formas de pago
					resultado = await CargaNivelesEstudio(loginToken.Token);
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine("Inicio carga de datos desde REX...");
					// Carga referencia formas de pago
					resultado = await CargaNivelesOcupacion(loginToken.Token);
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine("Inicio carga de datos desde REX...");
					// Carga referencia relaciones (vinculo familiar)
					resultado = await CargaVinculosFamiliares(loginToken.Token);
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine("Inicio carga de datos desde REX...");
					// Carga referencia instituciones (isapres, afp, otros)
					resultado = await CargaInstituciones(loginToken.Token);

					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine("Inicio carga de datos desde REX...");
					// Carga referencia terminos de contratos (finiquitos)
					resultado = await CargaTerminosContrato(loginToken.Token);

					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine("Inicio carga de datos desde REX...");
					// Carga referencia tipos de contratos (constantes)
					resultado = await CargaTiposContrato(loginToken.Token);

					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine("Inicio carga de datos desde REX...");
					// Carga referencia tipos de contratos (constantes)
					resultado = await CargaEstadosContrato(loginToken.Token);

				}

				// Carga cargos liquidacion (catalogos)
				resultado = new Resultado();
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio carga de datos desde REX...");
				// Carga cargos genericos desde catalogos
				resultado = await CargaCargosLiquidacion(loginToken.Token);

				// Carga cargos genericos unificados (catalogos)
				resultado = new Resultado();
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio carga de datos desde REX...");
				// Carga cargos genericos desde catalogos
				resultado = await CargaCargosGenericos(loginToken.Token);

				resultado = new Resultado();
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio carga de datos desde REX...");
				// Carga referencias 1 desde catalogos 
				resultado = await CargaReferencias1(loginToken.Token);

				resultado = new Resultado();
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio carga de datos desde REX...");
				// Carga referencias 2 desde catalogos 
				resultado = await CargaReferencias2(loginToken.Token);

				resultado = new Resultado();
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio carga de datos desde REX...");
				// Carga feriados desde catalogos 
				resultado = await CargaFeriados(loginToken.Token);

				resultado = new Resultado();
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio carga de datos desde REX...");
				// Carga feriados movi desde catalogos 
				resultado = await CargaFeriadosMovi(loginToken.Token);

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Inicio carga de datos desde REX...");
				// Carga centros de costo
				resultado = await CargaCentrosCosto(loginToken.Token);
				resultado = new Resultado();

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Inicio carga de datos desde REX...");
				//Carga colaboradores
				resultado = await CargaColaboradores(loginToken.Token);
				resultado = new Resultado();

				//**********************************************************************
				// Carga contratos de colaboradores
				// Aquí cargamos todos los contratos de un colaborador,
				// Independiente si ha tenido cambios o no en el último tiempo
				// Fué cvreado para la carga masiva de contratos de colaboradores,
				// de lo contrario solo carga o actualiza los ultimos según fecha de 
				// corte de colaborador.
				if (_miConfiguracion.TodosLosContratos.Equals("S"))
				{
					resultado = await CargaContratosColaboradorTodos(loginToken.Token);
					resultado = new Resultado();
				}
				else
				{
					resultado = await CargaContratosColaborador(loginToken.Token);
					resultado = new Resultado();
				}

				//**********************************************************************


				// Carga vacaciones
				resultado = await CargaVacaciones(loginToken.Token);
				resultado = new Resultado();

				// Carga licencias médicas
				resultado = await CargaLicenciasMedicas(loginToken.Token);
				resultado = new Resultado();

				// Carga Permisos
				resultado = await CargaPermisos(loginToken.Token);
				resultado = new Resultado();


			}

			return resultado;
		}


		/// <summary>
		/// Método para cargar las empresas desde la API de REX
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaEmpresas(string Token)
		{
			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de empresas...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<EmpresaRex> empresas = new List<EmpresaRex>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v2/empresas");
			request.Headers.Add("Authorization", "Token " + Token);
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectCatalogo
				var rootObjectCatalogo = JsonConvert.DeserializeObject<RootObjectEmpresa>(contents);
				empresas = rootObjectCatalogo.objetos;
				// Grabación de datos en SQL Server (Tabla de paso bancos)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de empresas...");
				string spName = "SP_REX_INSERTA_EMPRESA";
				foreach (var empresa in empresas)
				{
					if (!empresa.Empresa.Trim().Equals("todas"))
					{
						using (var conexion = new SqlConnection(_connectionString))
						{
							DynamicParameters dynamicParameters = new DynamicParameters();
							// Adding Input parameters.
							dynamicParameters.Add("@Nombre", empresa.Nombre);
							dynamicParameters.Add("@Empresa", empresa.Empresa);
							dynamicParameters.Add("@Rut", empresa.Rut);
							// Adding Output parameter.
							dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
							conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
							resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
							if (resultadoExeSql.Cantidad > 0)
							{
								resultadoExeSql.Estado = "OK";
								Console.ForegroundColor = ConsoleColor.Green;
								Console.WriteLine("Empresa creada correctamente en repositorio temporal : " + empresa.Nombre);
							}
							else
							{
								resultadoExeSql.Estado = "ERROR";
								Console.ForegroundColor = ConsoleColor.Red;
								Console.WriteLine("Error al ingresar empresa a repositorio temporal : " + empresa.Nombre);
							}
						}
					}
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}

		/// <summary>
		/// Método para cargar los bancos desde la API de REX (PARTE DE CATALOGOS)
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaBancos(string Token)
		{
			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de bancos...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<CatalogoRex> bancos = new List<CatalogoRex>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v3/catalogo/" + _miConfiguracion.CatalogoBanco + "?detalle=True&pagina=0");
			request.Headers.Add("Authorization", "Token " + Token);
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectCatalogo
				
				//var rootObjectCatalogo = JsonConvert.DeserializeObject<RootObjectCatalogo>(contents);
				//bancos = rootObjectCatalogo.objetos;

				bancos = JsonConvert.DeserializeObject<List<CatalogoRex>>(contents);

				// Grabación de datos en SQL Server (Tabla de paso bancos)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de bancos...");
				string spName = "SP_REX_INSERTA_CATALOGO";
				foreach (var banco in bancos)
				{
					using (var conexion = new SqlConnection(_connectionString))
					{
						DynamicParameters dynamicParameters = new DynamicParameters();
						// Adding Input parameters.
						dynamicParameters.Add("@Id", banco.Id);
						dynamicParameters.Add("@Fecha_Creacion", banco.Fecha_Creacion);
						dynamicParameters.Add("@Fecha_Modificacion", banco.Fecha_Modificacion);
						dynamicParameters.Add("@Item", banco.Item);
						dynamicParameters.Add("@Nombre", banco.Nombre);
						dynamicParameters.Add("@Valora", banco.Valora);
						dynamicParameters.Add("@Valorb", banco.Valorb);
						dynamicParameters.Add("@Valorc", banco.Valorc);
						dynamicParameters.Add("@Valord", banco.Valord);
						dynamicParameters.Add("@Valore", banco.Valore);
						dynamicParameters.Add("@Valorf", banco.Valorf);
						dynamicParameters.Add("@Valorg", banco.Valorg);
						dynamicParameters.Add("@Valorh", banco.Valorh);
						dynamicParameters.Add("@Valori", banco.Valori);
						dynamicParameters.Add("@Valorj", banco.Valorj);
						dynamicParameters.Add("@Valork", banco.Valork);
						dynamicParameters.Add("@Valorl", 0);
						dynamicParameters.Add("@DatoAdic", banco.DatoAdic);
						dynamicParameters.Add("@Lista", banco.Lista);
						dynamicParameters.Add("@Habilitado", banco.Habilitado);
						dynamicParameters.Add("@Reservado", banco.Reservado);
						// Adding Output parameter.
						dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
						conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
						resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
						if (resultadoExeSql.Cantidad > 0)
						{
							resultadoExeSql.Estado = "OK";
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Banco creado correctamente en repositorio temporal : " + banco.Nombre);
						}
						else
						{
							resultadoExeSql.Estado = "ERROR";
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error al ingresar banco a repositorio temporal : " + banco.Nombre);
						}
					}
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}

		/// <summary>
		/// Método para cargar las profesiones desde la API de REX  (PARTE DE CATALOGOS)
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaProfesiones(string Token)
		{
			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de profesiones...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<CatalogoRex> profesiones = new List<CatalogoRex>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v3/catalogo/" + _miConfiguracion.CatalogoProfesion + "?detalle=True&pagina=0");
			request.Headers.Add("Authorization", "Token " + Token);
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectCatalogo
				
				//var rootObjectCatalogo = JsonConvert.DeserializeObject<RootObjectCatalogo>(contents);
				//profesiones = rootObjectCatalogo.objetos;

				profesiones = JsonConvert.DeserializeObject<List<CatalogoRex>>(contents);

				// Grabación de datos en SQL Server (Tabla de paso profesiones)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de profesiones...");
				string spName = "SP_REX_INSERTA_CATALOGO";
				foreach (var profesion in profesiones)
				{
					using (var conexion = new SqlConnection(_connectionString))
					{
						DynamicParameters dynamicParameters = new DynamicParameters();
						// Adding Input parameters.
						dynamicParameters.Add("@Id", profesion.Id);
						dynamicParameters.Add("@Fecha_Creacion", profesion.Fecha_Creacion);
						dynamicParameters.Add("@Fecha_Modificacion", profesion.Fecha_Modificacion);
						dynamicParameters.Add("@Item", profesion.Item);
						dynamicParameters.Add("@Nombre", profesion.Nombre);
						dynamicParameters.Add("@Valora", profesion.Valora);
						dynamicParameters.Add("@Valorb", profesion.Valorb);
						dynamicParameters.Add("@Valorc", profesion.Valorc);
						dynamicParameters.Add("@Valord", profesion.Valord);
						dynamicParameters.Add("@Valore", profesion.Valore);
						dynamicParameters.Add("@Valorf", profesion.Valorf);
						dynamicParameters.Add("@Valorg", profesion.Valorg);
						dynamicParameters.Add("@Valorh", profesion.Valorh);
						dynamicParameters.Add("@Valori", profesion.Valori);
						dynamicParameters.Add("@Valorj", profesion.Valorj);
						dynamicParameters.Add("@Valork", profesion.Valork);
						dynamicParameters.Add("@Valorl", 0);
						dynamicParameters.Add("@DatoAdic", profesion.DatoAdic);
						dynamicParameters.Add("@Lista", profesion.Lista);
						dynamicParameters.Add("@Habilitado", profesion.Habilitado);
						dynamicParameters.Add("@Reservado", profesion.Reservado);
						// Adding Output parameter.
						dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
						conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
						resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
						if (resultadoExeSql.Cantidad > 0)
						{
							resultadoExeSql.Estado = "OK";
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Profesion creada correctamente en repositorio temporal : " + profesion.Nombre);
						}
						else
						{
							resultadoExeSql.Estado = "ERROR";
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error al ingresar profesion a repositorio temporal : " + profesion.Nombre);
						}
					}
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}

		/// <summary>
		/// Método para cargar las formas de pago desde la API de REX (PARTE DE CATALOGOS)
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaFormasPago(string Token)
		{
			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de formas de pago...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<CatalogoRex> formasPago = new List<CatalogoRex>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v3/catalogo/" + _miConfiguracion.CatalogoFormaPago + "?detalle=True&pagina=0");
			request.Headers.Add("Authorization", "Token " + Token);
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectCatalogo

				//var rootObjectCatalogo = JsonConvert.DeserializeObject<RootObjectCatalogo>(contents);
				//formasPago = rootObjectCatalogo.objetos;

				formasPago = JsonConvert.DeserializeObject<List<CatalogoRex>>(contents);

				// Grabación de datos en SQL Server (Tabla de paso catalogo)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de formas de pago...");
				string spName = "SP_REX_INSERTA_CATALOGO";
				foreach (var formaPago in formasPago)
				{
					using (var conexion = new SqlConnection(_connectionString))
					{
						DynamicParameters dynamicParameters = new DynamicParameters();
						// Adding Input parameters.
						dynamicParameters.Add("@Id", formaPago.Id);
						dynamicParameters.Add("@Fecha_Creacion", formaPago.Fecha_Creacion);
						dynamicParameters.Add("@Fecha_Modificacion", formaPago.Fecha_Modificacion);
						dynamicParameters.Add("@Item", formaPago.Item);
						dynamicParameters.Add("@Nombre", formaPago.Nombre);
						dynamicParameters.Add("@Valora", formaPago.Valora);
						dynamicParameters.Add("@Valorb", formaPago.Valorb);
						dynamicParameters.Add("@Valorc", formaPago.Valorc);
						dynamicParameters.Add("@Valord", formaPago.Valord);
						dynamicParameters.Add("@Valore", formaPago.Valore);
						dynamicParameters.Add("@Valorf", formaPago.Valorf);
						dynamicParameters.Add("@Valorg", formaPago.Valorg);
						dynamicParameters.Add("@Valorh", formaPago.Valorh);
						dynamicParameters.Add("@Valori", formaPago.Valori);
						dynamicParameters.Add("@Valorj", formaPago.Valorj);
						dynamicParameters.Add("@Valork", formaPago.Valork);
						dynamicParameters.Add("@Valorl", 0);
						dynamicParameters.Add("@DatoAdic", formaPago.DatoAdic);
						dynamicParameters.Add("@Lista", formaPago.Lista);
						dynamicParameters.Add("@Habilitado", formaPago.Habilitado);
						dynamicParameters.Add("@Reservado", formaPago.Reservado);
						// Adding Output parameter.
						dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
						conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
						resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
						if (resultadoExeSql.Cantidad > 0)
						{
							resultadoExeSql.Estado = "OK";
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Forma de pago correctamente en repositorio temporal : " + formaPago.Nombre);
						}
						else
						{
							resultadoExeSql.Estado = "ERROR";
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error al ingresar forma de pago a repositorio temporal : " + formaPago.Nombre);
						}
					}
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}

		/// <summary>
		/// Método para cargar los tipos de licencia de conducir desde la API de REX (PARTE DE CATALOGOS)
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaLicenciasConducir(string Token)
		{
			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de licencias de conducir...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<CatalogoRex> licenciasConducir = new List<CatalogoRex>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v3/catalogo/" + _miConfiguracion.CatalogoLicenciaConducir + "?detalle=True&pagina=0");
			request.Headers.Add("Authorization", "Token " + Token);
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectCatalogo

				//var rootObjectCatalogo = JsonConvert.DeserializeObject<RootObjectCatalogo>(contents);
				//licenciasConducir = rootObjectCatalogo.objetos;

				licenciasConducir = JsonConvert.DeserializeObject<List<CatalogoRex>>(contents);

				// Grabación de datos en SQL Server (Tabla de paso Catalogo)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de licencias de conducir...");
				string spName = "SP_REX_INSERTA_CATALOGO";
				foreach (var licenciaConducir in licenciasConducir)
				{
					using (var conexion = new SqlConnection(_connectionString))
					{
						DynamicParameters dynamicParameters = new DynamicParameters();
						// Adding Input parameters.
						dynamicParameters.Add("@Id", licenciaConducir.Id);
						dynamicParameters.Add("@Fecha_Creacion", licenciaConducir.Fecha_Creacion);
						dynamicParameters.Add("@Fecha_Modificacion", licenciaConducir.Fecha_Modificacion);
						dynamicParameters.Add("@Item", licenciaConducir.Item);
						dynamicParameters.Add("@Nombre", licenciaConducir.Nombre);
						dynamicParameters.Add("@Valora", licenciaConducir.Valora);
						dynamicParameters.Add("@Valorb", licenciaConducir.Valorb);
						dynamicParameters.Add("@Valorc", licenciaConducir.Valorc);
						dynamicParameters.Add("@Valord", licenciaConducir.Valord);
						dynamicParameters.Add("@Valore", licenciaConducir.Valore);
						dynamicParameters.Add("@Valorf", licenciaConducir.Valorf);
						dynamicParameters.Add("@Valorg", licenciaConducir.Valorg);
						dynamicParameters.Add("@Valorh", licenciaConducir.Valorh);
						dynamicParameters.Add("@Valori", licenciaConducir.Valori);
						dynamicParameters.Add("@Valorj", licenciaConducir.Valorj);
						dynamicParameters.Add("@Valork", licenciaConducir.Valork);
						dynamicParameters.Add("@Valorl", 0);
						dynamicParameters.Add("@DatoAdic", licenciaConducir.DatoAdic);
						dynamicParameters.Add("@Lista", licenciaConducir.Lista);
						dynamicParameters.Add("@Habilitado", licenciaConducir.Habilitado);
						dynamicParameters.Add("@Reservado", licenciaConducir.Reservado);
						// Adding Output parameter.
						dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
						conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
						resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
						if (resultadoExeSql.Cantidad > 0)
						{
							resultadoExeSql.Estado = "OK";
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Licencia de conducir creada correctamente en repositorio temporal : " + licenciaConducir.Nombre);
						}
						else
						{
							resultadoExeSql.Estado = "ERROR";
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error al ingresar licencia de conducir a repositorio temporal : " + licenciaConducir.Nombre);
						}
					}
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}

		/// <summary>
		/// Método para cargar los niveles de estudio desde la API de REX (PARTE DE CATALOGOS)
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaNivelesEstudio(string Token)
		{
			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de niveles de estudio...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<CatalogoRex> nivelesEstudio = new List<CatalogoRex>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v3/catalogo/" + _miConfiguracion.CatalogoNivelEstudio + "?detalle=True&pagina=0");
			request.Headers.Add("Authorization", "Token " + Token);
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectCatalogo

				//var rootObjectCatalogo = JsonConvert.DeserializeObject<RootObjectCatalogo>(contents);
				//nivelesEstudio = rootObjectCatalogo.objetos;

				nivelesEstudio = JsonConvert.DeserializeObject<List<CatalogoRex>>(contents);

				// Grabación de datos en SQL Server (Tabla de paso Catalogo)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de niveles de estudio...");
				string spName = "SP_REX_INSERTA_CATALOGO";
				foreach (var nivelEstudio in nivelesEstudio)
				{
					using (var conexion = new SqlConnection(_connectionString))
					{
						DynamicParameters dynamicParameters = new DynamicParameters();
						// Adding Input parameters.
						dynamicParameters.Add("@Id", nivelEstudio.Id);
						dynamicParameters.Add("@Fecha_Creacion", nivelEstudio.Fecha_Creacion);
						dynamicParameters.Add("@Fecha_Modificacion", nivelEstudio.Fecha_Modificacion);
						dynamicParameters.Add("@Item", nivelEstudio.Item);
						dynamicParameters.Add("@Nombre", nivelEstudio.Nombre);
						dynamicParameters.Add("@Valora", nivelEstudio.Valora);
						dynamicParameters.Add("@Valorb", nivelEstudio.Valorb);
						dynamicParameters.Add("@Valorc", nivelEstudio.Valorc);
						dynamicParameters.Add("@Valord", nivelEstudio.Valord);
						dynamicParameters.Add("@Valore", nivelEstudio.Valore);
						dynamicParameters.Add("@Valorf", nivelEstudio.Valorf);
						dynamicParameters.Add("@Valorg", nivelEstudio.Valorg);
						dynamicParameters.Add("@Valorh", nivelEstudio.Valorh);
						dynamicParameters.Add("@Valori", nivelEstudio.Valori);
						dynamicParameters.Add("@Valorj", nivelEstudio.Valorj);
						dynamicParameters.Add("@Valork", nivelEstudio.Valork);
						dynamicParameters.Add("@Valorl", 0);
						dynamicParameters.Add("@DatoAdic", nivelEstudio.DatoAdic);
						dynamicParameters.Add("@Lista", nivelEstudio.Lista);
						dynamicParameters.Add("@Habilitado", nivelEstudio.Habilitado);
						dynamicParameters.Add("@Reservado", nivelEstudio.Reservado);
						// Adding Output parameter.
						dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
						conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
						resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
						if (resultadoExeSql.Cantidad > 0)
						{
							resultadoExeSql.Estado = "OK";
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Nivel de estudio creado correctamente en repositorio temporal : " + nivelEstudio.Nombre);
						}
						else
						{
							resultadoExeSql.Estado = "ERROR";
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error al ingresar nivel de estudio a repositorio temporal : " + nivelEstudio.Nombre);
						}
					}
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}

		/// <summary>
		/// Método para cargar los niveles de ocupacion desde la API de REX (PARTE DE CATALOGOS)
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaNivelesOcupacion(string Token)
		{
			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de nivel ocupacional...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<CatalogoRex> nivelesOcupacion = new List<CatalogoRex>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v3/catalogo/" + _miConfiguracion.CatalogoNivelOcupacional + "?detalle=True&pagina=0");
			request.Headers.Add("Authorization", "Token " + Token);
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectCatalogo

				//var rootObjectCatalogo = JsonConvert.DeserializeObject<RootObjectCatalogo>(contents);
				//nivelesOcupacion = rootObjectCatalogo.objetos;

				nivelesOcupacion = JsonConvert.DeserializeObject<List<CatalogoRex>>(contents);

				// Grabación de datos en SQL Server (Tabla de paso Catalogo)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de niveles de ocupación...");
				string spName = "SP_REX_INSERTA_CATALOGO";
				foreach (var nivelOcupacion in nivelesOcupacion)
				{
					using (var conexion = new SqlConnection(_connectionString))
					{
						DynamicParameters dynamicParameters = new DynamicParameters();
						// Adding Input parameters.
						dynamicParameters.Add("@Id", nivelOcupacion.Id);
						dynamicParameters.Add("@Fecha_Creacion", nivelOcupacion.Fecha_Creacion);
						dynamicParameters.Add("@Fecha_Modificacion", nivelOcupacion.Fecha_Modificacion);
						dynamicParameters.Add("@Item", nivelOcupacion.Item);
						dynamicParameters.Add("@Nombre", nivelOcupacion.Nombre);
						dynamicParameters.Add("@Valora", nivelOcupacion.Valora);
						dynamicParameters.Add("@Valorb", nivelOcupacion.Valorb);
						dynamicParameters.Add("@Valorc", nivelOcupacion.Valorc);
						dynamicParameters.Add("@Valord", nivelOcupacion.Valord);
						dynamicParameters.Add("@Valore", nivelOcupacion.Valore);
						dynamicParameters.Add("@Valorf", nivelOcupacion.Valorf);
						dynamicParameters.Add("@Valorg", nivelOcupacion.Valorg);
						dynamicParameters.Add("@Valorh", nivelOcupacion.Valorh);
						dynamicParameters.Add("@Valori", nivelOcupacion.Valori);
						dynamicParameters.Add("@Valorj", nivelOcupacion.Valorj);
						dynamicParameters.Add("@Valork", nivelOcupacion.Valork);
						dynamicParameters.Add("@Valorl", 0);
						dynamicParameters.Add("@DatoAdic", nivelOcupacion.DatoAdic);
						dynamicParameters.Add("@Lista", nivelOcupacion.Lista);
						dynamicParameters.Add("@Habilitado", nivelOcupacion.Habilitado);
						dynamicParameters.Add("@Reservado", nivelOcupacion.Reservado);
						// Adding Output parameter.
						dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
						conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
						resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
						if (resultadoExeSql.Cantidad > 0)
						{
							resultadoExeSql.Estado = "OK";
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Nivel de ocupación creado correctamente en repositorio temporal : " + nivelOcupacion.Nombre);
						}
						else
						{
							resultadoExeSql.Estado = "ERROR";
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error al ingresar nivel de ocupación a repositorio temporal : " + nivelOcupacion.Nombre);
						}
					}
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}

		/// <summary>
		/// Método para cargar las relaciones (vinculo familiar) desde la API de REX (PARTE DE CATALOGOS)
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaVinculosFamiliares(string Token)
		{
			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de relaciones (vinculo familiar)...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<CatalogoRex> relaciones = new List<CatalogoRex>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v3/catalogo/" + _miConfiguracion.CatalogoRelaciones + "?detalle=True&pagina=0");
			request.Headers.Add("Authorization", "Token " + Token);
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectCatalogo

				//var rootObjectCatalogo = JsonConvert.DeserializeObject<RootObjectCatalogo>(contents);
				//relaciones = rootObjectCatalogo.objetos;

				relaciones = JsonConvert.DeserializeObject<List<CatalogoRex>>(contents);

				// Grabación de datos en SQL Server (Tabla de paso Catalogo)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de nrelaciones (vinculo familiar)...");
				string spName = "SP_REX_INSERTA_CATALOGO";
				foreach (var relacion in relaciones)
				{
					using (var conexion = new SqlConnection(_connectionString))
					{
						DynamicParameters dynamicParameters = new DynamicParameters();
						// Adding Input parameters.
						dynamicParameters.Add("@Id", relacion.Id);
						dynamicParameters.Add("@Fecha_Creacion", relacion.Fecha_Creacion);
						dynamicParameters.Add("@Fecha_Modificacion", relacion.Fecha_Modificacion);
						dynamicParameters.Add("@Item", relacion.Item);
						dynamicParameters.Add("@Nombre", relacion.Nombre);
						dynamicParameters.Add("@Valora", relacion.Valora);
						dynamicParameters.Add("@Valorb", relacion.Valorb);
						dynamicParameters.Add("@Valorc", relacion.Valorc);
						dynamicParameters.Add("@Valord", relacion.Valord);
						dynamicParameters.Add("@Valore", relacion.Valore);
						dynamicParameters.Add("@Valorf", relacion.Valorf);
						dynamicParameters.Add("@Valorg", relacion.Valorg);
						dynamicParameters.Add("@Valorh", relacion.Valorh);
						dynamicParameters.Add("@Valori", relacion.Valori);
						dynamicParameters.Add("@Valorj", relacion.Valorj);
						dynamicParameters.Add("@Valork", relacion.Valork);
						dynamicParameters.Add("@Valorl", 0);
						dynamicParameters.Add("@DatoAdic", relacion.DatoAdic);
						dynamicParameters.Add("@Lista", relacion.Lista);
						dynamicParameters.Add("@Habilitado", relacion.Habilitado);
						dynamicParameters.Add("@Reservado", relacion.Reservado);
						// Adding Output parameter.
						dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
						conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
						resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
						if (resultadoExeSql.Cantidad > 0)
						{
							resultadoExeSql.Estado = "OK";
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Vínculo familiar creado correctamente en repositorio temporal : " + relacion.Nombre);
						}
						else
						{
							resultadoExeSql.Estado = "ERROR";
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error al ingresar vínculo familiar a repositorio temporal : " + relacion.Nombre);
						}
					}
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}

		/// <summary>
		/// Método para cargar las causales de termino de contrato desde la API de REX (PARTE DE CATALOGOS)
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaTerminosContrato(string Token)
		{
			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de causales de termino de conmtrato (finiquitos)...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<CatalogoRex> causalesTerminoContrato = new List<CatalogoRex>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v3/catalogo/" + _miConfiguracion.CatalogoCausalesTerminoContrato + "?detalle=True&pagina=0");
			request.Headers.Add("Authorization", "Token " + Token);
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectCatalogo

				//var rootObjectCatalogo = JsonConvert.DeserializeObject<RootObjectCatalogo>(contents);
				//relaciones = rootObjectCatalogo.objetos;

				causalesTerminoContrato = JsonConvert.DeserializeObject<List<CatalogoRex>>(contents);

				// Grabación de datos en SQL Server (Tabla de paso Catalogo)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de causales de termino de contrato (finiquitos)...");
				string spName = "SP_REX_INSERTA_CATALOGO";
				foreach (var causalTerminoContrato in causalesTerminoContrato)
				{
					using (var conexion = new SqlConnection(_connectionString))
					{
						DynamicParameters dynamicParameters = new DynamicParameters();
						// Adding Input parameters.
						dynamicParameters.Add("@Id", causalTerminoContrato.Id);
						dynamicParameters.Add("@Fecha_Creacion", causalTerminoContrato.Fecha_Creacion);
						dynamicParameters.Add("@Fecha_Modificacion", causalTerminoContrato.Fecha_Modificacion);
						dynamicParameters.Add("@Item", causalTerminoContrato.Item);
						dynamicParameters.Add("@Nombre", causalTerminoContrato.Nombre);
						dynamicParameters.Add("@Valora", causalTerminoContrato.Valora);
						dynamicParameters.Add("@Valorb", causalTerminoContrato.Valorb);
						dynamicParameters.Add("@Valorc", causalTerminoContrato.Valorc);
						dynamicParameters.Add("@Valord", causalTerminoContrato.Valord);
						dynamicParameters.Add("@Valore", causalTerminoContrato.Valore);
						dynamicParameters.Add("@Valorf", causalTerminoContrato.Valorf);
						dynamicParameters.Add("@Valorg", causalTerminoContrato.Valorg);
						dynamicParameters.Add("@Valorh", causalTerminoContrato.Valorh);
						dynamicParameters.Add("@Valori", causalTerminoContrato.Valori);
						dynamicParameters.Add("@Valorj", causalTerminoContrato.Valorj);
						dynamicParameters.Add("@Valork", causalTerminoContrato.Valork);
						dynamicParameters.Add("@Valorl", 0);
						dynamicParameters.Add("@DatoAdic", causalTerminoContrato.DatoAdic);
						dynamicParameters.Add("@Lista", causalTerminoContrato.Lista);
						dynamicParameters.Add("@Habilitado", causalTerminoContrato.Habilitado);
						dynamicParameters.Add("@Reservado", causalTerminoContrato.Reservado);
						// Adding Output parameter.
						dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
						conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
						resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
						if (resultadoExeSql.Cantidad > 0)
						{
							resultadoExeSql.Estado = "OK";
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Causal termino de contarto creada correctamente en repositorio temporal : " + causalTerminoContrato.Nombre);
						}
						else
						{
							resultadoExeSql.Estado = "ERROR";
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error al ingresar causal de termino de contrato a repositorio temporal : " + causalTerminoContrato.Nombre);
						}
					}
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}

		/// <summary>
		/// Método para cargar los tipso de contrato desde la API de REX (PARTE DE CONSTANTES)
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaTiposContrato(string Token)
		{
			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de tipos de contrato...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<ConstanteRex> tiposContrato = new List<ConstanteRex>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v3/aliados/constantes/" + _miConfiguracion.ConstanteTipoContrato );
			request.Headers.Add("Authorization", "Token " + Token);
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectCatalogo

				//var rootObjectCatalogo = JsonConvert.DeserializeObject<RootObjectCatalogo>(contents);
				//relaciones = rootObjectCatalogo.objetos;

				var diccionario = JsonConvert.DeserializeObject<Dictionary<string, string>>(contents);
				tiposContrato = diccionario.Select(kvp => new ConstanteRex
				{
					Item = kvp.Key,
					Nombre = kvp.Value
				}).ToList();

				// Grabación de datos en SQL Server (Tabla de paso Catalogo)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de tipos de contrato...");
				string spName = "SP_REX_INSERTA_CONSTANTE";
				foreach (var tipoContrato in tiposContrato)
				{
					using (var conexion = new SqlConnection(_connectionString))
					{
						DynamicParameters dynamicParameters = new DynamicParameters();
						// Adding Input parameters.
						dynamicParameters.Add("@Item", tipoContrato.Item);
						dynamicParameters.Add("@Nombre", tipoContrato.Nombre);
						dynamicParameters.Add("@Lista", _miConfiguracion.ConstanteTipoContrato);
						// Adding Output parameter.
						dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
						conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
						resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
						if (resultadoExeSql.Cantidad > 0)
						{
							resultadoExeSql.Estado = "OK";
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Tipo de contrato creado correctamente en repositorio temporal : " + tipoContrato.Nombre);
						}
						else
						{
							resultadoExeSql.Estado = "ERROR";
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error al ingresar tipo de contrato a repositorio temporal : " + tipoContrato.Nombre);
						}
					}
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}

		/// <summary>
		/// Método para cargar los estados de contrato desde la API de REX (PARTE DE CONSTANTES)
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaEstadosContrato(string Token)
		{
			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de estados de contrato...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<ConstanteRex> estadosContrato = new List<ConstanteRex>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v3/aliados/constantes/" + _miConfiguracion.ConstanteEstadoContrato);
			request.Headers.Add("Authorization", "Token " + Token);
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectCatalogo

				//var rootObjectCatalogo = JsonConvert.DeserializeObject<RootObjectCatalogo>(contents);
				//relaciones = rootObjectCatalogo.objetos;

				var diccionario = JsonConvert.DeserializeObject<Dictionary<string, string>>(contents);
				estadosContrato = diccionario.Select(kvp => new ConstanteRex
				{
					Item = kvp.Key,
					Nombre = kvp.Value
				}).ToList();

				// Grabación de datos en SQL Server (Tabla de paso Catalogo)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de estados de contrato...");
				string spName = "SP_REX_INSERTA_CONSTANTE";
				foreach (var estadoContrato in estadosContrato)
				{
					using (var conexion = new SqlConnection(_connectionString))
					{
						DynamicParameters dynamicParameters = new DynamicParameters();
						// Adding Input parameters.
						dynamicParameters.Add("@Item", estadoContrato.Item);
						dynamicParameters.Add("@Nombre", estadoContrato.Nombre);
						dynamicParameters.Add("@Lista", _miConfiguracion.ConstanteEstadoContrato);
						// Adding Output parameter.
						dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
						conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
						resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
						if (resultadoExeSql.Cantidad > 0)
						{
							resultadoExeSql.Estado = "OK";
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Estado de contrato creado correctamente en repositorio temporal : " + estadoContrato.Nombre);
						}
						else
						{
							resultadoExeSql.Estado = "ERROR";
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error al ingresar estado de contrato a repositorio temporal : " + estadoContrato.Nombre);
						}
					}
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}

		/// <summary>
		/// Método para cargar las instituciones desde la API de REX
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaInstituciones(string Token)
		{
			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de instituciones...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<Institucions> instituciones = new List<Institucions>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v2/instituciones");
			request.Headers.Add("Authorization", "Token " + Token);
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectInstitucions
				var rootObjectInstitucions = JsonConvert.DeserializeObject<RootObjectInstitucions>(contents);
				instituciones = rootObjectInstitucions.objetos;
				// Grabación de datos en SQL Server (Tabla de paso Instituciones)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de instituciones...");
				string spName = "SP_REX_INSERTA_INSTITUCION";
				foreach (var institucion in instituciones)
				{
					using (var conexion = new SqlConnection(_connectionString))
					{
						DynamicParameters dynamicParameters = new DynamicParameters();
						// Adding Input parameters.
						dynamicParameters.Add("@Rut", institucion.Rut);
						dynamicParameters.Add("@Institucion", institucion.Institucion);
						dynamicParameters.Add("@Nombre", institucion.Nombre);
						dynamicParameters.Add("@Clasificacion", institucion.Clasificacion);
						// Adding Output parameter.
						dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
						conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
						resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
						if (resultadoExeSql.Cantidad > 0)
						{
							resultadoExeSql.Estado = "OK";
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Institución creada correctamente en repositorio temporal : " + institucion.Nombre);
						}
						else
						{
							resultadoExeSql.Estado = "ERROR";
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error al ingresar institución a repositorio temporal : " + institucion.Nombre);
						}
					}
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}


		///**************************************************************************************************///
		///**************************************************************************************************///
		///**************************************************************************************************///

		/// <summary>
		/// Carga registro de cargos liquidacion desde la API de REX 
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaCargosLiquidacion(string Token)
		{
			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de cargos liquidacion...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<CatalogoRex> cargosLiquidacion = new List<CatalogoRex>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v3/catalogo/" + _miConfiguracion.CatalogoCargo + "?detalle=True&pagina=0");
			request.Headers.Add("Authorization", "Token " + Token);
			//request.Headers.Add("Cookie", "csrftoken=46N0xuXLsR3JIYUUc1K5wMgcR78ipD1k");
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectCargo

				//var rootObjectCargo = JsonConvert.DeserializeObject<RootObjectCargo>(contents);
				//cargosLiquidacion = rootObjectCargo.objetos;

				cargosLiquidacion = JsonConvert.DeserializeObject<List<CatalogoRex>>(contents);

				// Grabación de datos en SQL Server (Tabla de paso catalogo)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de cargos liquidacion...");
				string spName = "SP_REX_INSERTA_CATALOGO";
				foreach (var cargoLiquidacion in cargosLiquidacion)
				{
					using (var conexion = new SqlConnection(_connectionString))
					{
						DynamicParameters dynamicParameters = new DynamicParameters();
						// Adding Input parameters.
						dynamicParameters.Add("@Id", cargoLiquidacion.Id);
						dynamicParameters.Add("@Fecha_Creacion", cargoLiquidacion.Fecha_Creacion);
						dynamicParameters.Add("@Fecha_Modificacion", cargoLiquidacion.Fecha_Modificacion);
						dynamicParameters.Add("@Item", cargoLiquidacion.Item);
						dynamicParameters.Add("@Nombre", cargoLiquidacion.Nombre);
						dynamicParameters.Add("@Valora", cargoLiquidacion.Valora);
						dynamicParameters.Add("@Valorb", cargoLiquidacion.Valorb);
						dynamicParameters.Add("@Valorc", cargoLiquidacion.Valorc);
						dynamicParameters.Add("@Valord", cargoLiquidacion.Valord);
						dynamicParameters.Add("@Valore", cargoLiquidacion.Valore);
						dynamicParameters.Add("@Valorf", cargoLiquidacion.Valorf);
						dynamicParameters.Add("@Valorg", cargoLiquidacion.Valorg);
						dynamicParameters.Add("@Valorh", cargoLiquidacion.Valorh);
						dynamicParameters.Add("@Valori", cargoLiquidacion.Valori);
						dynamicParameters.Add("@Valorj", cargoLiquidacion.Valorj);
						dynamicParameters.Add("@Valork", cargoLiquidacion.Valork);
						dynamicParameters.Add("@Valorl", cargoLiquidacion.Valorl);
						dynamicParameters.Add("@DatoAdic", cargoLiquidacion.DatoAdic);
						dynamicParameters.Add("@Lista", "cargoLiq");
						dynamicParameters.Add("@Habilitado", true);
						dynamicParameters.Add("@Reservado", false);
						// Adding Output parameter.
						dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
						conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
						resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
						if (resultadoExeSql.Cantidad > 0)
						{
							resultadoExeSql.Estado = "OK";
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Registro de cargo liquidación creado correctamente en repositorio temporal : " + cargoLiquidacion.Nombre);
						}
						else
						{
							resultadoExeSql.Estado = "ERROR";
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error al registrar cargo liquidación en repositorio temporal : " + cargoLiquidacion.Nombre);
						}
					}
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}

		/// <summary>
		/// Método para cargar los cargos genericos unificados desde la API de REX (PARTE DE CATALOGOS)
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaCargosGenericos(string Token)
		{
			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de cargos genericos...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<CatalogoRex> cargosGenericos = new List<CatalogoRex>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v3/catalogo/" + _miConfiguracion.CatalogoCargoGenericoUnificado + "?detalle=True&pagina=0");
			request.Headers.Add("Authorization", "Token " + Token);
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectCatalogo

				//var rootObjectCatalogo = JsonConvert.DeserializeObject<RootObjectCatalogo>(contents);
				//cargosGenericos = rootObjectCatalogo.objetos;

				cargosGenericos = JsonConvert.DeserializeObject<List<CatalogoRex>>(contents);

				// Grabación de datos en SQL Server (Tabla de paso catalogo)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de cargos genericos...");
				string spName = "SP_REX_INSERTA_CATALOGO";
				foreach (var cargoGenerico in cargosGenericos)
				{
					using (var conexion = new SqlConnection(_connectionString))
					{
						DynamicParameters dynamicParameters = new DynamicParameters();
						// Adding Input parameters.
						dynamicParameters.Add("@Id", cargoGenerico.Id);
						dynamicParameters.Add("@Fecha_Creacion", cargoGenerico.Fecha_Creacion);
						dynamicParameters.Add("@Fecha_Modificacion", cargoGenerico.Fecha_Modificacion);
						dynamicParameters.Add("@Item", cargoGenerico.Item);
						dynamicParameters.Add("@Nombre", cargoGenerico.Nombre);
						dynamicParameters.Add("@Valora", cargoGenerico.Valora);
						dynamicParameters.Add("@Valorb", cargoGenerico.Valorb);
						dynamicParameters.Add("@Valorc", cargoGenerico.Valorc);
						dynamicParameters.Add("@Valord", cargoGenerico.Valord);
						dynamicParameters.Add("@Valore", cargoGenerico.Valore);
						dynamicParameters.Add("@Valorf", cargoGenerico.Valorf);
						dynamicParameters.Add("@Valorg", cargoGenerico.Valorg);
						dynamicParameters.Add("@Valorh", cargoGenerico.Valorh);
						dynamicParameters.Add("@Valori", cargoGenerico.Valori);
						dynamicParameters.Add("@Valorj", cargoGenerico.Valorj);
						dynamicParameters.Add("@Valork", cargoGenerico.Valork);
						dynamicParameters.Add("@Valorl", cargoGenerico.Valorl);
						dynamicParameters.Add("@DatoAdic", cargoGenerico.DatoAdic);
						dynamicParameters.Add("@Lista", cargoGenerico.Lista);
						dynamicParameters.Add("@Habilitado", cargoGenerico.Habilitado);
						dynamicParameters.Add("@Reservado", cargoGenerico.Reservado);
						// Adding Output parameter.
						dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
						conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
						resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
						if (resultadoExeSql.Cantidad > 0)
						{
							resultadoExeSql.Estado = "OK";
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Cargo generico creado correctamente en repositorio temporal : " + cargoGenerico.Nombre);
						}
						else
						{
							resultadoExeSql.Estado = "ERROR";
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error al ingresar cargo generico a repositorio temporal : " + cargoGenerico.Nombre);
						}
					}
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}

		/// <summary>
		/// Método para cargar las referencias 1 desde la API de REX (PARTE DE CATALOGOS)
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaReferencias1(string Token)
		{
			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de referencias 1...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<CatalogoRex> referencias1 = new List<CatalogoRex>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v3/catalogo/" + _miConfiguracion.CatalogoReferencia1 + "?detalle=True&pagina=0");
			request.Headers.Add("Authorization", "Token " + Token);
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectCatalogo

				//var rootObjectCatalogo = JsonConvert.DeserializeObject<RootObjectCatalogo>(contents);
				//cargosGenericos = rootObjectCatalogo.objetos;

				referencias1 = JsonConvert.DeserializeObject<List<CatalogoRex>>(contents);

				// Grabación de datos en SQL Server (Tabla de paso catalogo)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de refrencias 1...");
				string spName = "SP_REX_INSERTA_CATALOGO";
				foreach (var referencia1 in referencias1)
				{
					using (var conexion = new SqlConnection(_connectionString))
					{
						DynamicParameters dynamicParameters = new DynamicParameters();
						// Adding Input parameters.
						dynamicParameters.Add("@Id", referencia1.Id);
						dynamicParameters.Add("@Fecha_Creacion", referencia1.Fecha_Creacion);
						dynamicParameters.Add("@Fecha_Modificacion", referencia1.Fecha_Modificacion);
						dynamicParameters.Add("@Item", referencia1.Item);
						dynamicParameters.Add("@Nombre", referencia1.Nombre);
						dynamicParameters.Add("@Valora", referencia1.Valora);
						dynamicParameters.Add("@Valorb", referencia1.Valorb);
						dynamicParameters.Add("@Valorc", referencia1.Valorc);
						dynamicParameters.Add("@Valord", referencia1.Valord);
						dynamicParameters.Add("@Valore", referencia1.Valore);
						dynamicParameters.Add("@Valorf", referencia1.Valorf);
						dynamicParameters.Add("@Valorg", referencia1.Valorg);
						dynamicParameters.Add("@Valorh", referencia1.Valorh);
						dynamicParameters.Add("@Valori", referencia1.Valori);
						dynamicParameters.Add("@Valorj", referencia1.Valorj);
						dynamicParameters.Add("@Valork", referencia1.Valork);
						dynamicParameters.Add("@Valorl", referencia1.Valorl);
						dynamicParameters.Add("@DatoAdic", referencia1.DatoAdic);
						dynamicParameters.Add("@Lista", referencia1.Lista);
						dynamicParameters.Add("@Habilitado", referencia1.Habilitado);
						dynamicParameters.Add("@Reservado", referencia1.Reservado);
						// Adding Output parameter.
						dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
						conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
						resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
						if (resultadoExeSql.Cantidad > 0)
						{
							resultadoExeSql.Estado = "OK";
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Referencia 1 creada correctamente en repositorio temporal : " + referencia1.Nombre);
						}
						else
						{
							resultadoExeSql.Estado = "ERROR";
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error al ingresar refrencia 1 a repositorio temporal : " + referencia1.Nombre);
						}
					}
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}

		/// <summary>
		/// Método para cargar las referencias 2 desde la API de REX (PARTE DE CATALOGOS)
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaReferencias2(string Token)
		{
			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de referencias 2...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<CatalogoRex> referencias2 = new List<CatalogoRex>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v3/catalogo/" + _miConfiguracion.CatalogoReferencia2 + "?detalle=True&pagina=0");
			request.Headers.Add("Authorization", "Token " + Token);
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectCatalogo

				//var rootObjectCatalogo = JsonConvert.DeserializeObject<RootObjectCatalogo>(contents);
				//cargosGenericos = rootObjectCatalogo.objetos;

				referencias2 = JsonConvert.DeserializeObject<List<CatalogoRex>>(contents);

				// Grabación de datos en SQL Server (Tabla de paso catalogo)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de refrencias 2...");
				string spName = "SP_REX_INSERTA_CATALOGO";
				foreach (var referencia2 in referencias2)
				{
					using (var conexion = new SqlConnection(_connectionString))
					{
						DynamicParameters dynamicParameters = new DynamicParameters();
						// Adding Input parameters.
						dynamicParameters.Add("@Id", referencia2.Id);
						dynamicParameters.Add("@Fecha_Creacion", referencia2.Fecha_Creacion);
						dynamicParameters.Add("@Fecha_Modificacion", referencia2.Fecha_Modificacion);
						dynamicParameters.Add("@Item", referencia2.Item);
						dynamicParameters.Add("@Nombre", referencia2.Nombre);
						dynamicParameters.Add("@Valora", referencia2.Valora);
						dynamicParameters.Add("@Valorb", referencia2.Valorb);
						dynamicParameters.Add("@Valorc", referencia2.Valorc);
						dynamicParameters.Add("@Valord", referencia2.Valord);
						dynamicParameters.Add("@Valore", referencia2.Valore);
						dynamicParameters.Add("@Valorf", referencia2.Valorf);
						dynamicParameters.Add("@Valorg", referencia2.Valorg);
						dynamicParameters.Add("@Valorh", referencia2.Valorh);
						dynamicParameters.Add("@Valori", referencia2.Valori);
						dynamicParameters.Add("@Valorj", referencia2.Valorj);
						dynamicParameters.Add("@Valork", referencia2.Valork);
						dynamicParameters.Add("@Valorl", referencia2.Valorl);
						dynamicParameters.Add("@DatoAdic", referencia2.DatoAdic);
						dynamicParameters.Add("@Lista", referencia2.Lista);
						dynamicParameters.Add("@Habilitado", referencia2.Habilitado);
						dynamicParameters.Add("@Reservado", referencia2.Reservado);
						// Adding Output parameter.
						dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
						conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
						resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
						if (resultadoExeSql.Cantidad > 0)
						{
							resultadoExeSql.Estado = "OK";
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Referencia 2 creada correctamente en repositorio temporal : " + referencia2.Nombre);
						}
						else
						{
							resultadoExeSql.Estado = "ERROR";
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error al ingresar refrencia 2 a repositorio temporal : " + referencia2.Nombre);
						}
					}
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}

		/// <summary>
		/// Método para cargar los feriados desde la API de REX (PARTE DE CATALOGOS)
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaFeriados(string Token)
		{
			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de feriados...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<CatalogoRex> feriados = new List<CatalogoRex>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v3/catalogo/" + _miConfiguracion.CatalogoFeriados + "?detalle=True&pagina=0");
			request.Headers.Add("Authorization", "Token " + Token);
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectCatalogo

				//var rootObjectCatalogo = JsonConvert.DeserializeObject<RootObjectCatalogo>(contents);
				//cargosGenericos = rootObjectCatalogo.objetos;

				feriados = JsonConvert.DeserializeObject<List<CatalogoRex>>(contents);

				// Grabación de datos en SQL Server (Tabla de paso catalogo)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de feriados...");
				string spName = "SP_REX_INSERTA_CATALOGO";
				foreach (var feriado in feriados)
				{
					using (var conexion = new SqlConnection(_connectionString))
					{
						DynamicParameters dynamicParameters = new DynamicParameters();
						// Adding Input parameters.
						dynamicParameters.Add("@Id", feriado.Id);
						dynamicParameters.Add("@Fecha_Creacion", feriado.Fecha_Creacion);
						dynamicParameters.Add("@Fecha_Modificacion", feriado.Fecha_Modificacion);
						dynamicParameters.Add("@Item", feriado.Item);
						dynamicParameters.Add("@Nombre", feriado.Nombre);
						dynamicParameters.Add("@Valora", feriado.Valora);
						dynamicParameters.Add("@Valorb", feriado.Valorb);
						dynamicParameters.Add("@Valorc", feriado.Valorc);
						dynamicParameters.Add("@Valord", feriado.Valord);
						dynamicParameters.Add("@Valore", feriado.Valore);
						dynamicParameters.Add("@Valorf", feriado.Valorf);
						dynamicParameters.Add("@Valorg", feriado.Valorg);
						dynamicParameters.Add("@Valorh", feriado.Valorh);
						dynamicParameters.Add("@Valori", feriado.Valori);
						dynamicParameters.Add("@Valorj", feriado.Valorj);
						dynamicParameters.Add("@Valork", feriado.Valork);
						dynamicParameters.Add("@Valorl", feriado.Valorl);
						dynamicParameters.Add("@DatoAdic", feriado.DatoAdic);
						dynamicParameters.Add("@Lista", feriado.Lista);
						dynamicParameters.Add("@Habilitado", feriado.Habilitado);
						dynamicParameters.Add("@Reservado", feriado.Reservado);
						// Adding Output parameter.
						dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
						conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
						resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
						if (resultadoExeSql.Cantidad > 0)
						{
							resultadoExeSql.Estado = "OK";
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Feriado creado correctamente en repositorio temporal : " + feriado.Nombre);
						}
						else
						{
							resultadoExeSql.Estado = "ERROR";
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error al ingresar feriado a repositorio temporal : " + feriado.Nombre);
						}
					}
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}

		/// <summary>
		/// Método para cargar los feriados movi desde la API de REX (PARTE DE CATALOGOS)
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaFeriadosMovi(string Token)
		{
			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de feriados movi...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<CatalogoRex> feriadosMovi = new List<CatalogoRex>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v3/catalogo/" + _miConfiguracion.CatalogoFeriadosMovi + "?detalle=True&pagina=0");
			request.Headers.Add("Authorization", "Token " + Token);
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectCatalogo

				//var rootObjectCatalogo = JsonConvert.DeserializeObject<RootObjectCatalogo>(contents);
				//cargosGenericos = rootObjectCatalogo.objetos;

				feriadosMovi = JsonConvert.DeserializeObject<List<CatalogoRex>>(contents);

				// Grabación de datos en SQL Server (Tabla de paso catalogo)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de feriados movi...");
				string spName = "SP_REX_INSERTA_CATALOGO";
				foreach (var feriadoMovi in feriadosMovi)
				{
					using (var conexion = new SqlConnection(_connectionString))
					{
						DynamicParameters dynamicParameters = new DynamicParameters();
						// Adding Input parameters.
						dynamicParameters.Add("@Id", feriadoMovi.Id);
						dynamicParameters.Add("@Fecha_Creacion", feriadoMovi.Fecha_Creacion);
						dynamicParameters.Add("@Fecha_Modificacion", feriadoMovi.Fecha_Modificacion);
						dynamicParameters.Add("@Item", feriadoMovi.Item);
						dynamicParameters.Add("@Nombre", feriadoMovi.Nombre);
						dynamicParameters.Add("@Valora", feriadoMovi.Valora);
						dynamicParameters.Add("@Valorb", feriadoMovi.Valorb);
						dynamicParameters.Add("@Valorc", feriadoMovi.Valorc);
						dynamicParameters.Add("@Valord", feriadoMovi.Valord);
						dynamicParameters.Add("@Valore", feriadoMovi.Valore);
						dynamicParameters.Add("@Valorf", feriadoMovi.Valorf);
						dynamicParameters.Add("@Valorg", feriadoMovi.Valorg);
						dynamicParameters.Add("@Valorh", feriadoMovi.Valorh);
						dynamicParameters.Add("@Valori", feriadoMovi.Valori);
						dynamicParameters.Add("@Valorj", feriadoMovi.Valorj);
						dynamicParameters.Add("@Valork", feriadoMovi.Valork);
						dynamicParameters.Add("@Valorl", feriadoMovi.Valorl);
						dynamicParameters.Add("@DatoAdic", feriadoMovi.DatoAdic);
						dynamicParameters.Add("@Lista", feriadoMovi.Lista);
						dynamicParameters.Add("@Habilitado", feriadoMovi.Habilitado);
						dynamicParameters.Add("@Reservado", feriadoMovi.Reservado);
						// Adding Output parameter.
						dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
						conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
						resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
						if (resultadoExeSql.Cantidad > 0)
						{
							resultadoExeSql.Estado = "OK";
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Feriado movi creado correctamente en repositorio temporal : " + feriadoMovi.Nombre);
						}
						else
						{
							resultadoExeSql.Estado = "ERROR";
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error al ingresar feriado movi a repositorio temporal : " + feriadoMovi.Nombre);
						}
					}
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}

		/// <summary>
		/// Carga los centros de costo desde la API de REX
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaCentrosCosto(string Token)
		{
			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de centros de costo...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<CentroCosto> centrosCosto = new List<CentroCosto>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v2/catalogo/" + _miConfiguracion.CatalogoCentroCosto + "?detalle=True");
			request.Headers.Add("Authorization", "Token " + Token);
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectCentroCosto
				var rootObjectCentroCosto = JsonConvert.DeserializeObject<RootObjectCentroCosto>(contents);
				centrosCosto = rootObjectCentroCosto.objetos;
				// Grabación de datos en SQL Server (Tabla de paso colaboradores)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de centros de costo...");
				string spName = "SP_REX_INSERTA_CENTRO_COSTO";
				foreach (var centrocosto in centrosCosto)
				{
					using (var conexion = new SqlConnection(_connectionString))
					{
						DynamicParameters dynamicParameters = new DynamicParameters();
						// Adding Input parameters.
						dynamicParameters.Add("@Id", centrocosto.Id);
						dynamicParameters.Add("@Fecha_Creacion", centrocosto.Fecha_Creacion);
						dynamicParameters.Add("@Fecha_Modificacion", centrocosto.Fecha_Modificacion);
						dynamicParameters.Add("@Item", centrocosto.Item);
						dynamicParameters.Add("@Nombre", centrocosto.Nombre);
						dynamicParameters.Add("@Valora", centrocosto.Valora);
						dynamicParameters.Add("@Valorb", centrocosto.Valorb);
						dynamicParameters.Add("@Valorc", centrocosto.Valorc);
						dynamicParameters.Add("@DatoAdic", centrocosto.DatoAdic);
						dynamicParameters.Add("@Lista", centrocosto.Lista);
						dynamicParameters.Add("@Habilitado", centrocosto.Habilitado);
						dynamicParameters.Add("@Reservado", centrocosto.Reservado);
						// Adding Output parameter.
						dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
						conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
						resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
						if (resultadoExeSql.Cantidad > 0)
						{
							resultadoExeSql.Estado = "OK";
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Centro de costo creado correctamente en repositorio temporal : " + centrocosto.Item);
						}
						else
						{
							resultadoExeSql.Estado = "ERROR";
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error al ingresar centro de costo a repositorio temporal : " + centrocosto.Item);
						}
					}
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}

		/// <summary>
		/// Carga los colaboradores desde la API de REX
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaColaboradores(string Token)
		{
			// Si existe fecha de corte, se utiliza para la consulta, sino se utiliza la fecha de ayer
			string FechaCorte = "";
			if (_miConfiguracion.FechaCorteColaborador.Trim().Equals(string.Empty)){
				FechaCorte = _miConfiguracion.FechaDiaAnterior.ToString("yyyy-MM-dd");
			} 
			else
			{
				FechaCorte = _miConfiguracion.FechaCorteColaborador;
			}

			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de colaboradores...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<Colaborador> colaboradores = new List<Colaborador>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v3/empleados?fechaCambio__gt=" + FechaCorte + "&paginar=0&pagina=&incluir_solo_aprobadores=");
			request.Headers.Add("Authorization", "Token " + Token);
			//request.Headers.Add("Cookie", "csrftoken=46N0xuXLsR3JIYUUc1K5wMgcR78ipD1k");
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectColaborador
				var rootObjectColaborador = JsonConvert.DeserializeObject<RootObjectColaborador>(contents);
				colaboradores = rootObjectColaborador.objetos;
				// Grabación de datos en SQL Server (Tabla de paso colaboradores)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de colaboradores...");
				string spName = "SP_REX_INSERTA_COLABORADOR";
				foreach (var colaborador in colaboradores)
				{
					using (var conexion = new SqlConnection(_connectionString))
					{
						DynamicParameters dynamicParameters = new DynamicParameters();
						// Adding Input parameters.
						dynamicParameters.Add("@Empleado", colaborador.Empleado);
						dynamicParameters.Add("@FormaPago", colaborador.FormaPago);
						dynamicParameters.Add("@Banco", colaborador.Banco);
						dynamicParameters.Add("@CuentaBanc", colaborador.CuentaBanc);
						dynamicParameters.Add("@Sucursal", colaborador.Sucursal);
						dynamicParameters.Add("@FechaCambio", colaborador.FechaCambio);
						dynamicParameters.Add("@Nombre", colaborador.Nombre);
						dynamicParameters.Add("@ApellidoPate", colaborador.ApellidoPate);
						dynamicParameters.Add("@ApellidoMate", colaborador.ApellidoMate);
						dynamicParameters.Add("@Sexo", colaborador.Sexo);
						dynamicParameters.Add("@FechaNaci", colaborador.FechaNaci);
						dynamicParameters.Add("@EstadoCivi", colaborador.EstadoCivi);
						dynamicParameters.Add("@Nacion", colaborador.Nacion);
						dynamicParameters.Add("@Direccion", colaborador.Direccion);
						dynamicParameters.Add("@Ciudad", colaborador.Ciudad);
						dynamicParameters.Add("@Region", colaborador.Region);
						dynamicParameters.Add("@NumeroFono", colaborador.NumeroFono);
						dynamicParameters.Add("@Email", colaborador.Email);
						dynamicParameters.Add("@ContratoActi", colaborador.ContratoActi);
						dynamicParameters.Add("@Supervisa", colaborador.Supervisa);
						dynamicParameters.Add("@Afp", colaborador.Afp);
						dynamicParameters.Add("@AfpCoti", colaborador.AfpCoti);
						dynamicParameters.Add("@Afp_Fecha_Afiliacion", colaborador.Afp_Fecha_Afiliacion);
						dynamicParameters.Add("@Jubilado", colaborador.Jubilado);
						dynamicParameters.Add("@Expatriado", colaborador.Expatriado);
						dynamicParameters.Add("@SistemaPens", colaborador.SistemaPens);
						dynamicParameters.Add("@Isapre", colaborador.Isapre);
						dynamicParameters.Add("@IsapreCoti", colaborador.IsapreCoti);
						dynamicParameters.Add("@IsaprePeso", colaborador.IsaprePeso);
						dynamicParameters.Add("@IsapreMone", colaborador.IsapreMone);
						dynamicParameters.Add("@IsapreFun", colaborador.IsapreFun);
						dynamicParameters.Add("@TramoAf", colaborador.TramoAf);
						dynamicParameters.Add("@CodigoInter", colaborador.CodigoInter);
						dynamicParameters.Add("@CodigoInte2", colaborador.CodigoInte2);
						dynamicParameters.Add("@Agrupacion", colaborador.Agrupacion);
						dynamicParameters.Add("@TallaRopa", colaborador.TallaRopa);
						dynamicParameters.Add("@TallaZapato", colaborador.TallaZapato);
						dynamicParameters.Add("@Talla_Pantalon", colaborador.Talla_Pantalon);
						dynamicParameters.Add("@Estudios", colaborador.Estudios);
						dynamicParameters.Add("@NumeroFono2", colaborador.NumeroFono2);
						dynamicParameters.Add("@Empresa", colaborador.Empresa);
						dynamicParameters.Add("@Situacion", colaborador.Situacion);
						dynamicParameters.Add("@Notas", colaborador.Notas);
						dynamicParameters.Add("@EmailPersonal", colaborador.EmailPersonal);
						dynamicParameters.Add("@NivelEstudios", colaborador.NivelEstudios);
						dynamicParameters.Add("@Profesion", colaborador.Profesion);
						dynamicParameters.Add("@LicenciaConducir", colaborador.LicenciaConducir);
						dynamicParameters.Add("@FechaCreacion", colaborador.FechaCreacion);
						dynamicParameters.Add("@Origen", colaborador.Origen);
						dynamicParameters.Add("@Cotiza_Prevision_Salud", colaborador.Cotiza_Prevision_Salud);
						dynamicParameters.Add("@Registrado_Dec", colaborador.Registrado_Dec);
						dynamicParameters.Add("@Es_Privado", colaborador.Es_Privado);
						dynamicParameters.Add("@Es_Solo_Aprovador", colaborador.Es_Solo_Aprovador);
						dynamicParameters.Add("@Es_Recontratable", colaborador.Es_Recontratable);
						dynamicParameters.Add("@Tipo_Documento", colaborador.Tipo_Documento);
						dynamicParameters.Add("@Nombre_Contacto_Emergencia", colaborador.Nombre_Contacto_Emergencia);
						dynamicParameters.Add("@Vinculo_Contacto_Emergencia", colaborador.Vinculo_Contacto_Emergencia);
						dynamicParameters.Add("@Telefono_Contacto_Emergencia", colaborador.Telefono_Contacto_Emergencia);
						dynamicParameters.Add("@Telefono2_Contacto_Emergencia", colaborador.Telefono2_Contacto_Emergencia);
						dynamicParameters.Add("@Nivel_Ocupacional", colaborador.Nivel_Ocupacional);
						dynamicParameters.Add("@Direccion_Completa", colaborador.Direccion_Completa);
						// Adding Output parameter.
						dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
						conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
						resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
						if (resultadoExeSql.Cantidad > 0)
						{
							resultadoExeSql.Estado = "OK";
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Colaborador creado correctamente en repositorio temporal : " + colaborador.Empleado);
						}
						else
						{
							resultadoExeSql.Estado = "ERROR";
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error al ingresar colaborador a repositorio temporal : " + colaborador.Empleado);
						}
					}
				}
				resultado.Estado = "OK";
			} else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}

		/// <summary>
		/// Carga los contratos de colaboradores desde la API de REX, previo cruce con colaboradores ya extraidos
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaContratosColaboradorTodos(string Token)
		{

			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<ListaContratosRex> listaContratos = new List<ListaContratosRex>();
			//RESCATAMOS DATOS DE TABLA DE COLABORADORES DE REX
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde paso colaboradores REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var colaboradores = conexion.Query<Colaborador>(@"
										   					    SET ROWCOUNT 1000 SELECT EMPLEADO FROM REX_COLABORADOR WHERE SW_CONTRATO = 'N' ");
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio carga de datos desde API REX de contratos por colaborador...");

				string spName = "SP_REX_INSERTA_CONTRATO";

				foreach (var colaborador in colaboradores)
				{
					string contents;
					var client = new HttpClient();
					var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v2/empleados/" + colaborador.Empleado.Trim() + "/contratos?fechaCambio__gt=&paginar=0&paginas=&empresa=");
					request.Headers.Add("Authorization", "Token " + Token);
					var content = new StringContent(string.Empty);
					content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
					request.Content = content;
					var response = await client.SendAsync(request);
					contents = await response.Content.ReadAsStringAsync();

					if (response.StatusCode.ToString().Equals("OK"))
					{
						// Deserializar el contenido JSON en un objeto de tipo RootObjectListaContratosRex
						var rootObjectListaContartosRex = JsonConvert.DeserializeObject<RootObjectListaContratosRex>(contents);
						listaContratos = rootObjectListaContartosRex.objetos;

						// Obtencion de detalle de contrato de colaborador
						foreach (var listaContrato in listaContratos)
						{
							request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v2/empleados/" + colaborador.Empleado.Trim() + "/contratos/" + listaContrato.Contrato.Trim());
							request.Headers.Add("Authorization", "Token " + Token);
							content = new StringContent(string.Empty);
							content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
							request.Content = content;
							response = await client.SendAsync(request);
							contents = await response.Content.ReadAsStringAsync();

							if (response.StatusCode.ToString().Equals("OK"))
							{
								// Deserializar el contenido JSON en un objeto de tipo Contrato
								var contrato = JsonConvert.DeserializeObject<ContratoRex>(contents);

								// Aqui debemos enviar los datos de los contratos a repositorio temporal


								using (var conexion2 = new SqlConnection(_connectionString))
								{
									DynamicParameters dynamicParameters = new DynamicParameters();
									// Adding Input parameters.
									dynamicParameters.Add("@Id", contrato.Id);
									dynamicParameters.Add("@FechaInic", contrato.FechaInic);
									dynamicParameters.Add("@FechaCambInde", contrato.FechaCambInde);
									dynamicParameters.Add("@VacacionesInic", contrato.VacacionesInic);
									dynamicParameters.Add("@ProgresivosInic", contrato.ProgresivosInic);
									dynamicParameters.Add("@FechaTerm", contrato.FechaTerm);
									dynamicParameters.Add("@FechaCesa", contrato.FechaCesa);
									dynamicParameters.Add("@ProgresivosReco", contrato.ProgresivosReco);
									dynamicParameters.Add("@Cargo", contrato.Cargo);
									dynamicParameters.Add("@Area", contrato.Area);

									dynamicParameters.Add("@Area_id", contrato.Area_id);
									dynamicParameters.Add("@Jefatura_directa_id", contrato.Jefatura_directa_id);
									dynamicParameters.Add("@Jefatura_indirecta_id", contrato.Jefatura_indirecta_id);
									dynamicParameters.Add("@Maneja_trato", contrato.Maneja_trato);
									dynamicParameters.Add("@SueldoBase", contrato.SueldoBase);
									dynamicParameters.Add("@FechaCambio", contrato.FechaCambio);
									dynamicParameters.Add("@Cargo_id", contrato.Cargo_id);
									dynamicParameters.Add("@Turno", contrato.Turno);
									dynamicParameters.Add("@Cotizacion", contrato.Cotizacion);
									dynamicParameters.Add("@Fecha_creacion", contrato.Fecha_creacion);

									dynamicParameters.Add("@Fecha_modificacion", contrato.Fecha_modificacion);
									dynamicParameters.Add("@Empresa", contrato.Empresa);
									dynamicParameters.Add("@Nombre", contrato.Nombre);
									dynamicParameters.Add("@Estado", contrato.Estado);
									dynamicParameters.Add("@TipoCont", contrato.TipoCont);
									dynamicParameters.Add("@Afecto_a_trato", contrato.Afecto_a_trato);
									dynamicParameters.Add("@Causal", contrato.Causal);
									dynamicParameters.Add("@CentroCost", contrato.CentroCost);
									dynamicParameters.Add("@Centro_distribucion", contrato.Centro_distribucion);
									dynamicParameters.Add("@Sede", contrato.Sede);

									dynamicParameters.Add("@PlantillaGrup", contrato.PlantillaGrup);
									dynamicParameters.Add("@TrabajoPesa", contrato.TrabajoPesa);
									dynamicParameters.Add("@TrabajoPesaPorc", contrato.TrabajoPesaPorc);
									dynamicParameters.Add("@Sindicato", contrato.Sindicato);
									dynamicParameters.Add("@JornadaParc", contrato.JornadaParc);
									dynamicParameters.Add("@Pacto_sueldo_unidad", contrato.Pacto_sueldo_unidad);
									dynamicParameters.Add("@Pacto_sueldo_valor", contrato.Pacto_sueldo_valor);
									dynamicParameters.Add("@HorasSema", contrato.HorasSema);
									dynamicParameters.Add("@DistribucionJorn", contrato.DistribucionJorn);
									dynamicParameters.Add("@Modalidad_contrato", contrato.Modalidad_contrato);

									dynamicParameters.Add("@Permite_ausencias_inhabiles", contrato.Permite_ausencias_inhabiles);
									dynamicParameters.Add("@SeguroCesa", contrato.SeguroCesa);
									dynamicParameters.Add("@Agrupacion", contrato.Agrupacion);
									dynamicParameters.Add("@CategoriaIne", contrato.CategoriaIne);
									dynamicParameters.Add("@Pauta", contrato.Pauta);
									dynamicParameters.Add("@Supervisor", contrato.Supervisor);
									dynamicParameters.Add("@SueldoPatr", contrato.SueldoPatr);
									dynamicParameters.Add("@DetalleCont", contrato.DetalleCont);
									dynamicParameters.Add("@Lta01", contrato.Lta01);
									dynamicParameters.Add("@Lta02", contrato.Lta02);

									dynamicParameters.Add("@Lta03", contrato.Lta03);
									dynamicParameters.Add("@Lta04", contrato.Lta04);
									dynamicParameters.Add("@Lta05", contrato.Lta05);
									dynamicParameters.Add("@Lta06", contrato.Lta06);
									dynamicParameters.Add("@Lta07", contrato.Lta07);
									dynamicParameters.Add("@Lta08", contrato.Lta08);
									dynamicParameters.Add("@Lta09", contrato.Lta09);
									dynamicParameters.Add("@Lta10", contrato.Lta10);
									dynamicParameters.Add("@Lta11", contrato.Lta11);
									dynamicParameters.Add("@Lta12", contrato.Lta12);

									dynamicParameters.Add("@Lta13", contrato.Lta13);
									dynamicParameters.Add("@Lta14", contrato.Lta14);
									dynamicParameters.Add("@Lta15", contrato.Lta15);
									dynamicParameters.Add("@Lta16", contrato.Lta16);
									dynamicParameters.Add("@Lta17", contrato.Lta17);
									dynamicParameters.Add("@Lta18", contrato.Lta18);
									dynamicParameters.Add("@Lta19", contrato.Lta19);
									dynamicParameters.Add("@Lta20", contrato.Lta20);
									dynamicParameters.Add("@Zona_extrema", contrato.Zona_extrema);
									dynamicParameters.Add("@Fca01", contrato.Fca01);

									dynamicParameters.Add("@Fca02", contrato.Fca02);
									dynamicParameters.Add("@Cpa01", contrato.Cpa01);
									dynamicParameters.Add("@Cpa02", contrato.Cpa02);
									dynamicParameters.Add("@NivelSence", contrato.NivelSence);
									dynamicParameters.Add("@FactorSence", contrato.FactorSence);
									dynamicParameters.Add("@Identificador_externo", contrato.Identificador_externo);
									dynamicParameters.Add("@Idarchivo", contrato.Idarchivo);
									dynamicParameters.Add("@Descansa_domingos", contrato.Descansa_domingos);
									dynamicParameters.Add("@Caja_compensacion", contrato.Caja_compensacion);
									dynamicParameters.Add("@Forma_pago_contrato", contrato.Forma_pago_contrato);

									dynamicParameters.Add("@Mes_reinicio_administrativos", contrato.Mes_reinicio_administrativos);
									dynamicParameters.Add("@Direccion_laboral", contrato.Direccion_laboral);
									dynamicParameters.Add("@Es_reemplazo", contrato.Es_reemplazo);
									dynamicParameters.Add("@Tiene_subsidio_licencia", contrato.Tiene_subsidio_licencia);
									dynamicParameters.Add("@Fecha_renovacion", contrato.Fecha_renovacion);
									dynamicParameters.Add("@Fecha_renovacion_2", contrato.Fecha_renovacion_2);
									dynamicParameters.Add("@Utiliza_asistencia", contrato.Utiliza_asistencia);
									dynamicParameters.Add("@Ultimo_proceso_reajuste", contrato.Ultimo_proceso_reajuste);
									dynamicParameters.Add("@Acceso_liquidacion_portal", contrato.Acceso_liquidacion_portal);
									dynamicParameters.Add("@Origen_instrumento_colectivo", contrato.Origen_instrumento_colectivo);

									dynamicParameters.Add("@Identificador_instrumento", contrato.Identificador_instrumento);
									dynamicParameters.Add("@Reconocimiento_antiguedad", contrato.Reconocimiento_antiguedad);
									dynamicParameters.Add("@Empleado", contrato.Empleado);
									dynamicParameters.Add("@Contrato", contrato.Contrato);
									dynamicParameters.Add("@Borrador", contrato.Borrador);
									dynamicParameters.Add("@Jefatura_directa", contrato.Jefatura_directa);
									dynamicParameters.Add("@Jefatura_indirecta", contrato.Jefatura_indirecta);


									// Adding Output parameter.
									dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
									conexion2.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
									resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
									if (resultadoExeSql.Cantidad > 0)
									{
										resultadoExeSql.Estado = "OK";
										Console.ForegroundColor = ConsoleColor.Green;
										Console.WriteLine("Contrato creado correctamente en repositorio temporal : " + colaborador.Empleado + " contrato Id: " + contrato.Id.ToString());
									}
									else
									{
										resultadoExeSql.Estado = "ERROR";
										Console.ForegroundColor = ConsoleColor.Red;
										Console.WriteLine("Error al ingresar contrato a repositorio temporal : " + colaborador.Empleado + " contrato Id: " + contrato.Id.ToString());
									}
								}

								




							}
							else
							{
								Console.WriteLine(response.StatusCode.ToString() + "Status para la obtencion de datos de contrato de colaborador: " + colaborador.Empleado);
								
							}
							resultado.Estado = response.StatusCode.ToString();
						}

						// Actualizamos en la tabla de colaboradores que ya tiene contrato
						using (var conexion3 = new SqlConnection(_connectionString))
						{
							var ejecuta = conexion3.Execute("UPDATE REX_COLABORADOR SET SW_CONTRATO = 'S' WHERE LTRIM(RTRIM(EMPLEADO)) = '" + colaborador.Empleado + "'");
						}

					}
					else
					{
						Console.WriteLine(response.StatusCode.ToString() + "Status para la obtencion de lista de contartos de colaborador: " + colaborador.Empleado);
						
					}
					resultado.Estado = response.StatusCode.ToString();
				}
			}

			if(!resultado.Estado.Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Error al ingresar contartos a repositorio temporal");
			}

			return resultado;

		}

		/// <summary>
		/// Carga los contratos de colaboradores desde la API de REX
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaContratosColaborador(string Token)
		{
			// Si existe fecha de corte, se utiliza para la consulta, sino se utiliza la fecha de ayer
			string FechaCorte = "";
			if (_miConfiguracion.FechaCorteColaborador.Trim().Equals(string.Empty))
			{
				FechaCorte = _miConfiguracion.FechaDiaAnterior.ToString("yyyy-MM-dd");
			}
			else
			{
				FechaCorte = _miConfiguracion.FechaCorteColaborador;
			}

			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de contratos...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<ContratoRex> listaContratos = new List<ContratoRex>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v2/contratos?paginar=0&fechaCambio__gt=" + FechaCorte);
			request.Headers.Add("Authorization", "Token " + Token);
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectColaborador
				var rootObjectContrato = JsonConvert.DeserializeObject<RootObjectContratosRex>(contents);
				listaContratos = rootObjectContrato.objetos;
				// Grabación de datos en SQL Server (Tabla de paso colaboradores)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de contratos...");
				string spName = "SP_REX_INSERTA_CONTRATO";
				foreach (var contrato in listaContratos)
				{
					using (var conexion = new SqlConnection(_connectionString))
					{
						DynamicParameters dynamicParameters = new DynamicParameters();
						// Adding Input parameters.
						dynamicParameters.Add("@Id", contrato.Id);
						dynamicParameters.Add("@FechaInic", contrato.FechaInic);
						dynamicParameters.Add("@FechaCambInde", contrato.FechaCambInde);
						dynamicParameters.Add("@VacacionesInic", contrato.VacacionesInic);
						dynamicParameters.Add("@ProgresivosInic", contrato.ProgresivosInic);
						dynamicParameters.Add("@FechaTerm", contrato.FechaTerm);
						dynamicParameters.Add("@FechaCesa", contrato.FechaCesa);
						dynamicParameters.Add("@ProgresivosReco", contrato.ProgresivosReco);
						dynamicParameters.Add("@Cargo", contrato.Cargo);
						dynamicParameters.Add("@Area", contrato.Area);

						dynamicParameters.Add("@Area_id", contrato.Area_id);
						dynamicParameters.Add("@Jefatura_directa_id", contrato.Jefatura_directa_id);
						dynamicParameters.Add("@Jefatura_indirecta_id", contrato.Jefatura_indirecta_id);
						dynamicParameters.Add("@Maneja_trato", contrato.Maneja_trato);
						dynamicParameters.Add("@SueldoBase", contrato.SueldoBase);
						dynamicParameters.Add("@FechaCambio", contrato.FechaCambio);
						dynamicParameters.Add("@Cargo_id", contrato.Cargo_id);
						dynamicParameters.Add("@Turno", contrato.Turno);
						dynamicParameters.Add("@Cotizacion", contrato.Cotizacion);
						dynamicParameters.Add("@Fecha_creacion", contrato.Fecha_creacion);

						dynamicParameters.Add("@Fecha_modificacion", contrato.Fecha_modificacion);
						dynamicParameters.Add("@Empresa", contrato.Empresa);
						dynamicParameters.Add("@Nombre", contrato.Nombre);
						dynamicParameters.Add("@Estado", contrato.Estado);
						dynamicParameters.Add("@TipoCont", contrato.TipoCont);
						dynamicParameters.Add("@Afecto_a_trato", contrato.Afecto_a_trato);
						dynamicParameters.Add("@Causal", contrato.Causal);
						dynamicParameters.Add("@CentroCost", contrato.CentroCost);
						dynamicParameters.Add("@Centro_distribucion", contrato.Centro_distribucion);
						dynamicParameters.Add("@Sede", contrato.Sede);

						dynamicParameters.Add("@PlantillaGrup", contrato.PlantillaGrup);
						dynamicParameters.Add("@TrabajoPesa", contrato.TrabajoPesa);
						dynamicParameters.Add("@TrabajoPesaPorc", contrato.TrabajoPesaPorc);
						dynamicParameters.Add("@Sindicato", contrato.Sindicato);
						dynamicParameters.Add("@JornadaParc", contrato.JornadaParc);
						dynamicParameters.Add("@Pacto_sueldo_unidad", contrato.Pacto_sueldo_unidad);
						dynamicParameters.Add("@Pacto_sueldo_valor", contrato.Pacto_sueldo_valor);
						dynamicParameters.Add("@HorasSema", contrato.HorasSema);
						dynamicParameters.Add("@DistribucionJorn", contrato.DistribucionJorn);
						dynamicParameters.Add("@Modalidad_contrato", contrato.Modalidad_contrato);

						dynamicParameters.Add("@Permite_ausencias_inhabiles", contrato.Permite_ausencias_inhabiles);
						dynamicParameters.Add("@SeguroCesa", contrato.SeguroCesa);
						dynamicParameters.Add("@Agrupacion", contrato.Agrupacion);
						dynamicParameters.Add("@CategoriaIne", contrato.CategoriaIne);
						dynamicParameters.Add("@Pauta", contrato.Pauta);
						dynamicParameters.Add("@Supervisor", contrato.Supervisor);
						dynamicParameters.Add("@SueldoPatr", contrato.SueldoPatr);
						dynamicParameters.Add("@DetalleCont", contrato.DetalleCont);
						dynamicParameters.Add("@Lta01", contrato.Lta01);
						dynamicParameters.Add("@Lta02", contrato.Lta02);

						dynamicParameters.Add("@Lta03", contrato.Lta03);
						dynamicParameters.Add("@Lta04", contrato.Lta04);
						dynamicParameters.Add("@Lta05", contrato.Lta05);
						dynamicParameters.Add("@Lta06", contrato.Lta06);
						dynamicParameters.Add("@Lta07", contrato.Lta07);
						dynamicParameters.Add("@Lta08", contrato.Lta08);
						dynamicParameters.Add("@Lta09", contrato.Lta09);
						dynamicParameters.Add("@Lta10", contrato.Lta10);
						dynamicParameters.Add("@Lta11", contrato.Lta11);
						dynamicParameters.Add("@Lta12", contrato.Lta12);

						dynamicParameters.Add("@Lta13", contrato.Lta13);
						dynamicParameters.Add("@Lta14", contrato.Lta14);
						dynamicParameters.Add("@Lta15", contrato.Lta15);
						dynamicParameters.Add("@Lta16", contrato.Lta16);
						dynamicParameters.Add("@Lta17", contrato.Lta17);
						dynamicParameters.Add("@Lta18", contrato.Lta18);
						dynamicParameters.Add("@Lta19", contrato.Lta19);
						dynamicParameters.Add("@Lta20", contrato.Lta20);
						dynamicParameters.Add("@Zona_extrema", contrato.Zona_extrema);
						dynamicParameters.Add("@Fca01", contrato.Fca01);

						dynamicParameters.Add("@Fca02", contrato.Fca02);
						dynamicParameters.Add("@Cpa01", contrato.Cpa01);
						dynamicParameters.Add("@Cpa02", contrato.Cpa02);
						dynamicParameters.Add("@NivelSence", contrato.NivelSence);
						dynamicParameters.Add("@FactorSence", contrato.FactorSence);
						dynamicParameters.Add("@Identificador_externo", contrato.Identificador_externo);
						dynamicParameters.Add("@Idarchivo", contrato.Idarchivo);
						dynamicParameters.Add("@Descansa_domingos", contrato.Descansa_domingos);
						dynamicParameters.Add("@Caja_compensacion", contrato.Caja_compensacion);
						dynamicParameters.Add("@Forma_pago_contrato", contrato.Forma_pago_contrato);

						dynamicParameters.Add("@Mes_reinicio_administrativos", contrato.Mes_reinicio_administrativos);
						dynamicParameters.Add("@Direccion_laboral", contrato.Direccion_laboral);
						dynamicParameters.Add("@Es_reemplazo", contrato.Es_reemplazo);
						dynamicParameters.Add("@Tiene_subsidio_licencia", contrato.Tiene_subsidio_licencia);
						dynamicParameters.Add("@Fecha_renovacion", contrato.Fecha_renovacion);
						dynamicParameters.Add("@Fecha_renovacion_2", contrato.Fecha_renovacion_2);
						dynamicParameters.Add("@Utiliza_asistencia", contrato.Utiliza_asistencia);
						dynamicParameters.Add("@Ultimo_proceso_reajuste", contrato.Ultimo_proceso_reajuste);
						dynamicParameters.Add("@Acceso_liquidacion_portal", contrato.Acceso_liquidacion_portal);
						dynamicParameters.Add("@Origen_instrumento_colectivo", contrato.Origen_instrumento_colectivo);

						dynamicParameters.Add("@Identificador_instrumento", contrato.Identificador_instrumento);
						dynamicParameters.Add("@Reconocimiento_antiguedad", contrato.Reconocimiento_antiguedad);
						dynamicParameters.Add("@Empleado", contrato.Empleado);
						dynamicParameters.Add("@Contrato", contrato.Contrato);
						dynamicParameters.Add("@Borrador", contrato.Borrador);
						dynamicParameters.Add("@Jefatura_directa", contrato.Jefatura_directa);
						dynamicParameters.Add("@Jefatura_indirecta", contrato.Jefatura_indirecta);


						// Adding Output parameter.
						dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
						conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
						resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
						if (resultadoExeSql.Cantidad > 0)
						{
							resultadoExeSql.Estado = "OK";
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Contrato creado correctamente en repositorio temporal : " + contrato.Empleado + " contrato Id: " + contrato.Id.ToString());
						}
						else
						{
							resultadoExeSql.Estado = "ERROR";
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error al ingresar contrato a repositorio temporal : " + contrato.Empleado + " contrato Id: " + contrato.Id.ToString());
						}
					}
				}
				resultado.Estado = "OK";

			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();

			}
			return resultado;

		}

		/// <summary>
		/// Carga registro de vacaciones desde la API de REX 
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaVacaciones(string Token)
		{
			// Si existe fecha de inicio, se utiliza para la consulta, sino se utiliza la fecha de un mes de antelación
			// La fecha de termino del periodo es de 2 meses hacia adelante 
			string FechaInicioVacacion = "";
			string FechaTerminoVacacion = "";
			if (_miConfiguracion.FechaInicioVacacion.Trim().Equals(string.Empty))
			{
				FechaInicioVacacion = _miConfiguracion.FechaDiaAnterior.AddMonths(-1).ToString("yyyy-MM-dd"); // 1 mes hacia atras 
			}
			else
			{
				FechaInicioVacacion = _miConfiguracion.FechaInicioVacacion;
			}
			FechaTerminoVacacion = _miConfiguracion.FechaActual.Date.AddMonths(12).ToString("yyyy-MM-dd"); // 12 meses hacia adelante



			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de registro de vacaciones...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<Vacacion> vacaciones = new List<Vacacion>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v2/vacaciones?fecha_inicio=" + FechaInicioVacacion + "&fecha_fin=" + FechaTerminoVacacion);
			request.Headers.Add("Authorization", "Token " + Token);
			//request.Headers.Add("Cookie", "csrftoken=46N0xuXLsR3JIYUUc1K5wMgcR78ipD1k");
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectVacacion
				var rootObjectVacacion = JsonConvert.DeserializeObject<RootObjectVacacion>(contents);
				vacaciones = rootObjectVacacion.objetos;
				// Grabación de datos en SQL Server (Tabla de paso vacaciones)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de vacaciones...");
				string spName = "SP_REX_INSERTA_VACACION";
				foreach (var vacacion in vacaciones)
				{
					using (var conexion = new SqlConnection(_connectionString))
					{
						DynamicParameters dynamicParameters = new DynamicParameters();
						// Adding Input parameters.
						dynamicParameters.Add("@Id", vacacion.Id);
						dynamicParameters.Add("@Empleado", vacacion.Empleado);
						dynamicParameters.Add("@Contrato", vacacion.Contrato);
						dynamicParameters.Add("@TipoMovi", vacacion.TipoMovil);
						dynamicParameters.Add("@Calificacion", vacacion.Calificacion);
						dynamicParameters.Add("@Dias", vacacion.Dias);
						dynamicParameters.Add("@FechaInic", vacacion.FechaInic);
						dynamicParameters.Add("@FechaTerm", vacacion.FechaTerm);
						dynamicParameters.Add("@FechaRetorno", vacacion.FechaRetorno);
						dynamicParameters.Add("@HoraInicio", vacacion.HoraInicio);
						dynamicParameters.Add("@Motivo", vacacion.Motivo);
						dynamicParameters.Add("@Tipo_Medio_Dia", vacacion.Tipo_Medio_Dia);
						dynamicParameters.Add("@Firmado", vacacion.Firmado);
						dynamicParameters.Add("@Valor_Dia_Progresivo", vacacion.Valor_Dia_Progresivo);
						dynamicParameters.Add("@Fichero_Id", vacacion.Fichero_Id);					
						// Adding Output parameter.
						dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
						conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
						resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
						if (resultadoExeSql.Cantidad > 0)
						{
							resultadoExeSql.Estado = "OK";
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Registro de vacación creado correctamente en repositorio temporal : " + vacacion.Empleado);
						}
						else
						{
							resultadoExeSql.Estado = "ERROR";
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error al registrar vacación en repositorio temporal : " + vacacion.Empleado);
						}
					}
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}

		/// <summary>
		/// Carga registro de licencias médicas desde la API de REX 
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaLicenciasMedicas(string Token)
		{
			// Si existe fecha de inicio, se utiliza para la consulta, sino se utiliza la fecha de un mes de anticipacion
			// La fecha de termino del periodo es de 1 año hacia adelante
			string FechaInicioLicenciaMedica = "";
			string FechaTerminoLicenciaMedica = "";
			if (_miConfiguracion.FechaInicioLicenciaMedica.Trim().Equals(string.Empty))
			{
				FechaInicioLicenciaMedica = _miConfiguracion.FechaDiaAnterior.AddMonths(-1).ToString("yyyy-MM-dd"); // 1 mes hacia atras 
			}
			else
			{
				FechaInicioLicenciaMedica = _miConfiguracion.FechaInicioLicenciaMedica;
			}
			FechaTerminoLicenciaMedica = _miConfiguracion.FechaActual.Date.AddMonths(12).ToString("yyyy-MM-dd"); // 12 meses hacia adelante



			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de registro de licencias medicas...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<LicenciaMedica> licencias = new List<LicenciaMedica>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v3/ausentismos/licencias_medicas?fecha_inicio=" + FechaInicioLicenciaMedica + "&fecha_termino=" + FechaTerminoLicenciaMedica + "&detalle=True");
			request.Headers.Add("Authorization", "Token " + Token);
			//request.Headers.Add("Cookie", "csrftoken=46N0xuXLsR3JIYUUc1K5wMgcR78ipD1k");
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectLicenciaMedica
				var rootObjectLicenciaMedica = JsonConvert.DeserializeObject<RootObjectLicenciaMedica>(contents);
				licencias = rootObjectLicenciaMedica.objetos;
				// Grabación de datos en SQL Server (Tabla de paso licencias medicas)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de licencias medicas...");
				string spName = "SP_REX_INSERTA_LICENCIA_MEDICA";
				foreach (var licencia in licencias)
				{

                    foreach (var contrato in licencia.Contratos)
                    {
						using (var conexion = new SqlConnection(_connectionString))
						{
							DynamicParameters dynamicParameters = new DynamicParameters();
							// Adding Input parameters.
							dynamicParameters.Add("@Id", licencia.Id);
							dynamicParameters.Add("@Contrato", contrato);
							dynamicParameters.Add("@Fecha_Ingreso", licencia.Fecha_Ingreso);
							dynamicParameters.Add("@Fecha_Inicio", licencia.Fecha_Inicio);
							dynamicParameters.Add("@Fecha_Termino", licencia.Fecha_Termino);
							dynamicParameters.Add("@Dias", licencia.Dias);
							dynamicParameters.Add("@Tipo_Medio_Dia", licencia.Tipo_Medio_Dia);
							dynamicParameters.Add("@Tipo_Licencia", licencia.Tipo_Licencia);
							dynamicParameters.Add("@Numero_Licencia", licencia.Numero_Licencia);
							dynamicParameters.Add("@Tipo_Ingreso", licencia.Tipo_Ingreso);
							dynamicParameters.Add("@Fecha_Calculo", licencia.Fecha_Calculo);
							dynamicParameters.Add("@Fecha_Aplicacion", licencia.Fecha_Aplicacion);
							dynamicParameters.Add("@Licencia_Anterior", licencia.Licencia_Anterior);
							dynamicParameters.Add("@Fecha_Licencia_Inicial", licencia.Fecha_Licencia_Inicial);
							dynamicParameters.Add("@Total_Dias_Continuacion", licencia.Total_Dias_Continuacion);
							dynamicParameters.Add("@Descripcion", licencia.Descripcion);
							dynamicParameters.Add("@Subtipo_Licencia", licencia.Subtipo_Licencia);
							dynamicParameters.Add("@Tipo_Licencia_Inicial", licencia.Tipo_Licencia_Inicial);
							dynamicParameters.Add("@Medico_Tratante", licencia.Medico_Tratante);
							dynamicParameters.Add("@Identificador_Medico_Tratante", licencia.Identificador_Medico_Tratante);
							dynamicParameters.Add("@Especialidad_Medico", licencia.Especialidad_Medico);
							dynamicParameters.Add("@Nombre_Especialidad_Medico", licencia.Nombre_Especialidad_Medico);
							dynamicParameters.Add("@Dias_A_Pagar", licencia.Dias_A_Pagar);
							dynamicParameters.Add("@No_Rebaja", licencia.No_Rebaja);
							// Adding Output parameter.
							dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
							conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
							resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
							if (resultadoExeSql.Cantidad > 0)
							{
								resultadoExeSql.Estado = "OK";
								Console.ForegroundColor = ConsoleColor.Green;
								Console.WriteLine("Registro de licencia medica creado correctamente en repositorio temporal : " + licencia.Id);
							}
							else
							{
								resultadoExeSql.Estado = "ERROR";
								Console.ForegroundColor = ConsoleColor.Red;
								Console.WriteLine("Error al registrar licencia medica en repositorio temporal : " + licencia.Id);
							}
						}
					}
					resultado.Estado = "OK";
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}

		/// <summary>
		/// Carga registro de licencias médicas desde la API de REX 
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		public async Task<Resultado> CargaPermisos(string Token)
		{
			// Si existe fecha de inicio, se utiliza para la consulta, sino se utiliza la fecha de un mes de anticipacion
			// La fecha de termino del periodo es de 1 año hacia adelante
			string FechaInicioPermiso = "";
			string FechaTerminoPermiso = "";
			if (_miConfiguracion.FechaInicioPermisos.Trim().Equals(string.Empty))
			{
				FechaInicioPermiso = _miConfiguracion.FechaDiaAnterior.AddMonths(-1).ToString("yyyy-MM-dd"); // 1 mes hacia atras 
			}
			else
			{
				FechaInicioPermiso = _miConfiguracion.FechaInicioPermisos;
			}
			FechaTerminoPermiso = _miConfiguracion.FechaActual.Date.AddMonths(12).ToString("yyyy-MM-dd"); // 12 meses hacia adelante



			// Obtención de datos desde REX mediante llamada API 
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Consultando datos de registro de permisos...");
			Resultado resultado = new Resultado();
			ResultadoExeSql resultadoExeSql = new ResultadoExeSql();
			List<Permiso> permisos = new List<Permiso>();
			string contents;
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, _miConfiguracion.UrlBase + "/api/v3/ausentismos/permisos?fecha_inicio=" + FechaInicioPermiso + "&fecha_termino=" + FechaTerminoPermiso + "&paginar=0&detalle=True");
			request.Headers.Add("Authorization", "Token " + Token);
			//request.Headers.Add("Cookie", "csrftoken=46N0xuXLsR3JIYUUc1K5wMgcR78ipD1k");
			var content = new StringContent(string.Empty);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			contents = await response.Content.ReadAsStringAsync();

			if (response.StatusCode.ToString().Equals("OK"))
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Consulta finalizada correctamente...");
				// Deserializar el contenido JSON en un objeto de tipo RootObjectPermiso
				var rootObjectPermiso = JsonConvert.DeserializeObject<RootObjectPermiso>(contents);
				permisos = rootObjectPermiso.objetos;
				// Grabación de datos en SQL Server (Tabla de paso permisos)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Regitrando información temporal de permisos...");
				string spName = "SP_REX_INSERTA_PERMISO";
				foreach (var permiso in permisos)
				{
					foreach (var contrato in permiso.Contratos)
					{
						using (var conexion = new SqlConnection(_connectionString))
						{
							DynamicParameters dynamicParameters = new DynamicParameters();
							// Adding Input parameters.
							dynamicParameters.Add("@Id", permiso.Id);
							dynamicParameters.Add("@Contrato", contrato);
							dynamicParameters.Add("@Fecha_Ingreso", permiso.Fecha_Ingreso);
							dynamicParameters.Add("@Fecha_Inicio", permiso.Fecha_Inicio);
							dynamicParameters.Add("@Fecha_Termino", permiso.Fecha_Termino);
							dynamicParameters.Add("@Dias", permiso.Dias);
							dynamicParameters.Add("@Tipo", permiso.Tipo);
							dynamicParameters.Add("@Tipo_Nombre", permiso.Tipo_Nombre);
							dynamicParameters.Add("@Subtipo_Permiso", permiso.Subtipo_Permiso);
							dynamicParameters.Add("@Subtipo_Permiso_Nombre", permiso.Subtipo_Permiso_Nombre);
							dynamicParameters.Add("@Goce_Sueldo", permiso.Goce_Sueldo);
							dynamicParameters.Add("@Descripcion", permiso.Descripcion);
							dynamicParameters.Add("@Fecha_Aplicacion", permiso.Fecha_Aplicacion);
							dynamicParameters.Add("@Tipo_Medio_Dia", permiso.Tipo_Medio_Dia);
							// Adding Output parameter.
							dynamicParameters.Add("@CANTIDAD", DbType.Int32, direction: ParameterDirection.Output);
							conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
							resultadoExeSql.Cantidad = dynamicParameters.Get<int>("@CANTIDAD");
							if (resultadoExeSql.Cantidad > 0)
							{
								resultadoExeSql.Estado = "OK";
								Console.ForegroundColor = ConsoleColor.Green;
								Console.WriteLine("Registro de permiso creado correctamente en repositorio temporal : " + permiso.Id);
							}
							else
							{
								resultadoExeSql.Estado = "ERROR";
								Console.ForegroundColor = ConsoleColor.Red;
								Console.WriteLine("Error al registrar permiso en repositorio temporal : " + permiso.Id);
							}
						}
					}
					resultado.Estado = "OK";
				}
				resultado.Estado = "OK";
			}
			else
			{
				resultado.Estado = response.StatusCode.ToString();
			}
			return resultado;
		}
	}
}
