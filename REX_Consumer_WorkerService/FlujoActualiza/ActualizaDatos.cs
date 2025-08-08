using Dapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using REX_Consumer_WorkerService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace REX_Consumer_WorkerService.FlujoActualiza
{
	public class ActualizaDatos
	{
		private readonly string _connectionString = "";
		private readonly MiConfiguracion _miConfiguracion;
		private Resultado resultado = new Resultado();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="configuration"></param>
		public ActualizaDatos(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");

			_miConfiguracion = new MiConfiguracion
			{
				AplicaConstantes = configuration["MiConfiguracion:AplicaConstantes"],
			};
		}

		public async Task<Resultado> ActualizaDatosBD()
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Inicio Actualizacion de datos desde REX a SQL...");

			// Validación para actualizar información parametrica
			if (_miConfiguracion.AplicaConstantes.Trim().Equals("S"))
			{
				// Actualiza Empresas
				resultado = await ActualizacionEmpresas();
				if (resultado.Estado == "OK")
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Actualizacion de empresas finalizada correctamente.");
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Actualizacion de empresas finalizada con errores.");
				}

				// Actualiza Paises
				resultado = await ActualizacionPaises();
				if (resultado.Estado == "OK")
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Actualizacion de paises finalizada correctamente.");
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Actualizacion de paises finalizada con errores.");
				}

				// Actualiza Bancos
				resultado = await ActualizacionBancos();
				if (resultado.Estado == "OK")
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Actualizacion de bancos finalizada correctamente.");
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Actualizacion de bancos finalizada con errores.");
				}

				// Actualiza Formas de Pago
				resultado = await ActualizacionFormasPago();
				if (resultado.Estado == "OK")
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Actualizacion de formas de pago (abonos) finalizada correctamente.");
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Actualizacion de formas de pago (abonos) finalizada con errores.");
				}

				// Actualiza AFP
				resultado = await ActualizacionAFP();
				if (resultado.Estado == "OK")
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Actualizacion de AFP finalizada correctamente.");
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Actualizacion de AFP finalizada con errores.");
				}

				// Actualiza SALUD
				resultado = await ActualizacionSALUD();
				if (resultado.Estado == "OK")
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Actualizacion de previsiónes de salud finalizada correctamente.");
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Actualizacion de previsiónes de salud finalizada con errores.");
				}

				// Actualiza Licencia de conducir
				resultado = await ActualizacionLicenciaConducir();
				if (resultado.Estado == "OK")
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Actualizacion de licencias de conducir finalizada correctamente.");
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Actualizacion de licencias de conducir finalizada con errores.");
				}

				// Actualiza nivel de estudio
				resultado = await ActualizacionNivelEstudio();
				if (resultado.Estado == "OK")
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Actualizacion de niveles de estudio finalizada correctamente.");
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Actualizacion de niveles de estudio finalizada con errores.");
				}

				// Actualiza vinculos familiares
				resultado = await ActualizacionVinculoFamiliar();
				if (resultado.Estado == "OK")
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Actualizacion de vinculos familiares finalizada correctamente.");
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Actualizacion de vinculos familiares finalizada con errores.");
				}


				// Actualiza terminos de contrato
				resultado = await ActualizacionTerminoContrato();
				if (resultado.Estado == "OK")
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Actualizacion de términos de contrato finalizada correctamente.");
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Actualizacion de término de contratos finalizada con errores.");
				}

				// Actualiza tipos de contrato
				resultado = await ActualizacionTipoContrato();
				if (resultado.Estado == "OK")
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Actualizacion de tipos de contrato finalizada correctamente.");
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Actualizacion de tipos de contrato finalizada con errores.");
				}

				// Actualiza estados de contrato
				resultado = await ActualizacionEstadoContrato();
				if (resultado.Estado == "OK")
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Actualizacion de estados de contrato finalizada correctamente.");
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Actualizacion de estados de contrato finalizada con errores.");
				}



				// Actualiza profesiones
				resultado = await ActualizacionProfesion();
				if (resultado.Estado == "OK")
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Actualizacion de profesiones finalizada correctamente.");
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Actualizacion de profesiones finalizada con errores.");
				}

				// Actualiza niveles ocupacionales
				resultado = await ActualizacionNivelOcupacional();
				if (resultado.Estado == "OK")
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Actualizacion de niveles ocupacionales finalizado correctamente.");
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Actualizacion de niveles ocupacionales finalizado con errores.");
				}

			}

			//Actualizacion cargos liquidacion
			resultado = await ActualizacionCargoLiquidacion();
			if (resultado.Estado == "OK")
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Actualizacion de cargos liquidacion finalizado correctamente.");
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Actualizacion de cargos liquidacion finalizado con errores.");
			}

			//Actualizacion cargos genericos
			resultado = await ActualizacionCargoGenerico();
			if (resultado.Estado == "OK")
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Actualizacion de cargos genericos finalizado correctamente.");
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Actualizacion de cargos genericos finalizado con errores.");
			}

			//Actualizacion refrencias 1
			resultado = await ActualizacionReferencia1();
			if (resultado.Estado == "OK")
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Actualizacion de referencias 1 finalizado correctamente.");
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Actualizacion de referencias 1 finalizado con errores.");
			}

			//Actualizacion refrencias 2
			resultado = await ActualizacionReferencia2();
			if (resultado.Estado == "OK")
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Actualizacion de referencias 2 finalizado correctamente.");
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Actualizacion de referencias 2 finalizado con errores.");
			}

			//Actualizacion de centros de costo
			resultado = await ActualizacionCentroCosto();
			if (resultado.Estado == "OK")
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Actualizacion de centros de costo finalizado correctamente.");
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Actualizacion de centros de costo finalizado con errores.");
			}


			//Actualiza información de colaboradores
			resultado = await ActualizacionColaborador();
			if (resultado.Estado == "OK")
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Actualizacion de colaboradores finalizado correctamente.");
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Actualizacion de colaboradores finalizado con errores.");
			}

			//Actualiza información de contratos de colaboradores
			resultado = await ActualizacionContratoColaborador();
			if (resultado.Estado == "OK")
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Actualizacion de contratos de colaboradores finalizado correctamente.");
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Actualizacion de contratos de colaboradores finalizado con errores.");
			}

			return resultado;


		}

		public async Task<Resultado> ActualizacionEmpresas()
		{
			Resultado resultado = new Resultado();
			ResultadoExeSqlInsUpd resultadoExeSqlInsUpd = new ResultadoExeSqlInsUpd();
			//RESCATAMOS DATOS DE TABLA DE EMPRESAS DE REX
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde Paso Empresas REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var empresas = conexion.Query<EmpresaRex>(@"
													     SELECT * FROM REX_EMPRESA ORDER BY CONVERT(INT,EMPRESA) ");

				string spName = "SP_REXSQL_INS_UPD_EMPRESA";

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio actualización de datos de empresas hacia SQL...");

				foreach (var empresa in empresas)
				{

					DynamicParameters dynamicParameters = new DynamicParameters();
					// Adding Input parameters.
					dynamicParameters.Add("@EMPRESA", empresa.Empresa);
					dynamicParameters.Add("@NOMBRE", empresa.Nombre);
					// Adding Output parameter.
					dynamicParameters.Add("@CANTIDAD_INS", DbType.Int32, direction: ParameterDirection.Output);
					dynamicParameters.Add("@CANTIDAD_ACT", DbType.Int32, direction: ParameterDirection.Output);
					conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
					resultadoExeSqlInsUpd.CantidadIns = dynamicParameters.Get<int>("@CANTIDAD_INS");
					resultadoExeSqlInsUpd.CantidadUpd = dynamicParameters.Get<int>("@CANTIDAD_ACT");
					if (resultadoExeSqlInsUpd.CantidadIns > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Empresa creada en base relacional : " + empresa.Nombre);
					}
					else if (resultadoExeSqlInsUpd.CantidadUpd > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Empresa actualizada en base relacional : " + empresa.Nombre);
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Error al actualizar o crear empresa : " + empresa.Nombre);
						resultado.Estado = "NO-OK";
					}
				}

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Termino de actualizacion de datos de empresas hacia SQL...");
			}

			return resultado;
		}

		public async Task<Resultado> ActualizacionPaises()
		{
			Resultado resultado = new Resultado();
			ResultadoExeSqlInsUpd resultadoExeSqlInsUpd = new ResultadoExeSqlInsUpd();
			//RESCATAMOS DATOS DE TABLA DE PAISES DE REX
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde Paso Pasies REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var paises = conexion.Query<PaisREX>(@"
													   SELECT * FROM REX_PAIS");

				string spName = "SP_REXSQL_INS_UPD_PAIS";

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio actualización de datos de paises hacia SQL...");

				foreach (var pais in paises)
				{

					DynamicParameters dynamicParameters = new DynamicParameters();
					// Adding Input parameters.
					dynamicParameters.Add("@ITEM", pais.Item);
					dynamicParameters.Add("@NOMBRE", pais.Nombre);
					// Adding Output parameter.
					dynamicParameters.Add("@CANTIDAD_INS", DbType.Int32, direction: ParameterDirection.Output);
					dynamicParameters.Add("@CANTIDAD_ACT", DbType.Int32, direction: ParameterDirection.Output);
					conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
					resultadoExeSqlInsUpd.CantidadIns = dynamicParameters.Get<int>("@CANTIDAD_INS");
					resultadoExeSqlInsUpd.CantidadUpd = dynamicParameters.Get<int>("@CANTIDAD_ACT");
					if (resultadoExeSqlInsUpd.CantidadIns > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Pais creado en base relacional : " + pais.Nombre);
					}
					else if (resultadoExeSqlInsUpd.CantidadUpd > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Pais actualizado en base relacional : " + pais.Nombre);
					}else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Error al actualizar o crear pais : " + pais.Nombre);
						resultado.Estado = "NO-OK";
					}
				}

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Termino de actualizacion de datos de paises hacia SQL...");
			}

			return resultado;
		}

		public async Task<Resultado> ActualizacionBancos()
		{
			Resultado resultado = new Resultado();
			ResultadoExeSqlInsUpd resultadoExeSqlInsUpd = new ResultadoExeSqlInsUpd();
			//RESCATAMOS DATOS DE TABLA DE PAISES DE REX
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde Paso Bancos REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var bancos = conexion.Query<CatalogoRex>(@"
													     SELECT * FROM REX_CATALOGO WHERE LISTA = 'bancos' ");

				string spName = "SP_REXSQL_INS_UPD_BANCO";

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio actualización de datos de bancos hacia SQL...");

				foreach (var banco in bancos)
				{

					DynamicParameters dynamicParameters = new DynamicParameters();
					// Adding Input parameters.
					dynamicParameters.Add("@ID", banco.Id);
					dynamicParameters.Add("@ITEM", banco.Item);
					dynamicParameters.Add("@NOMBRE", banco.Nombre);
					if (banco.Habilitado)
					{
						dynamicParameters.Add("@VIGENCIA", "S");
					}
					else
					{
						dynamicParameters.Add("@VIGENCIA", "N");
					}
					// Adding Output parameter.
					dynamicParameters.Add("@CANTIDAD_INS", DbType.Int32, direction: ParameterDirection.Output);
					dynamicParameters.Add("@CANTIDAD_ACT", DbType.Int32, direction: ParameterDirection.Output);
					conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
					resultadoExeSqlInsUpd.CantidadIns = dynamicParameters.Get<int>("@CANTIDAD_INS");
					resultadoExeSqlInsUpd.CantidadUpd = dynamicParameters.Get<int>("@CANTIDAD_ACT");
					if (resultadoExeSqlInsUpd.CantidadIns > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Banco creado en base relacional : " + banco.Nombre);
					}
					else if (resultadoExeSqlInsUpd.CantidadUpd > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Banco actualizado en base relacional : " + banco.Nombre);
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Error al actualizar o crear banco : " + banco.Nombre);
						resultado.Estado = "NO-OK";
					}
				}

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Termino de actualizacion de datos de bancos hacia SQL...");
			}

			return resultado;
		}

		public async Task<Resultado> ActualizacionFormasPago()
		{
			Resultado resultado = new Resultado();
			ResultadoExeSqlInsUpd resultadoExeSqlInsUpd = new ResultadoExeSqlInsUpd();
			//RESCATAMOS DATOS DE TABLA DE PAISES DE REX
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde Paso Formas de Pago REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var formasPago = conexion.Query<CatalogoRex>(@"
													     SELECT * FROM REX_CATALOGO WHERE LISTA = 'formasPago' ");

				string spName = "SP_REXSQL_INS_UPD_TIPO_ABONO";

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio actualización de datos de formas de pago hacia SQL...");

				foreach (var formaPago in formasPago)
				{

					DynamicParameters dynamicParameters = new DynamicParameters();
					// Adding Input parameters.
					dynamicParameters.Add("@ID", formaPago.Id);
					dynamicParameters.Add("@ITEM", formaPago.Item);
					dynamicParameters.Add("@NOMBRE", formaPago.Nombre);
					if (formaPago.Habilitado)
					{
						dynamicParameters.Add("@VIGENCIA", "S");
					}
					else
					{
						dynamicParameters.Add("@VIGENCIA", "N");
					}
					// Adding Output parameter.
					dynamicParameters.Add("@CANTIDAD_INS", DbType.Int32, direction: ParameterDirection.Output);
					dynamicParameters.Add("@CANTIDAD_ACT", DbType.Int32, direction: ParameterDirection.Output);
					conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
					resultadoExeSqlInsUpd.CantidadIns = dynamicParameters.Get<int>("@CANTIDAD_INS");
					resultadoExeSqlInsUpd.CantidadUpd = dynamicParameters.Get<int>("@CANTIDAD_ACT");
					if (resultadoExeSqlInsUpd.CantidadIns > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Forma de pago creada en base relacional : " + formaPago.Nombre);
					}
					else if (resultadoExeSqlInsUpd.CantidadUpd > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Forma de pago actualizada en base relacional : " + formaPago.Nombre);
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Error al actualizar o crear forma de pago : " + formaPago.Nombre);
						resultado.Estado = "NO-OK";
					}
				}

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Termino de actualizacion de datos de formas de pago hacia SQL...");
			}

			return resultado;
		}

		public async Task<Resultado> ActualizacionAFP()
		{
			Resultado resultado = new Resultado();
			ResultadoExeSqlInsUpd resultadoExeSqlInsUpd = new ResultadoExeSqlInsUpd();
			//RESCATAMOS DATOS DE TABLA DE PAISES DE REX
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde Paso Instituciones AFP REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var afps = conexion.Query<Institucions>(@"
													      SELECT * FROM REX_INSTITUCION WHERE CLASIFICACION = 'af' ");

				string spName = "SP_REXSQL_INS_UPD_AFP";

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio actualización de datos de instituciones AFP hacia SQL...");

				foreach (var afp in afps)
				{

					DynamicParameters dynamicParameters = new DynamicParameters();
					// Adding Input parameters.
					dynamicParameters.Add("@INSTITUCION", afp.Institucion);
					dynamicParameters.Add("@NOMBRE", afp.Nombre);
					// Adding Output parameter.
					dynamicParameters.Add("@CANTIDAD_INS", DbType.Int32, direction: ParameterDirection.Output);
					dynamicParameters.Add("@CANTIDAD_ACT", DbType.Int32, direction: ParameterDirection.Output);
					conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
					resultadoExeSqlInsUpd.CantidadIns = dynamicParameters.Get<int>("@CANTIDAD_INS");
					resultadoExeSqlInsUpd.CantidadUpd = dynamicParameters.Get<int>("@CANTIDAD_ACT");
					if (resultadoExeSqlInsUpd.CantidadIns > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("AFP creada en base relacional : " + afp.Nombre);
					}
					else if (resultadoExeSqlInsUpd.CantidadUpd > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("AFP actualizada en base relacional : " + afp.Nombre);
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Error al actualizar o crear AFP : " + afp.Nombre);
						resultado.Estado = "NO-OK";
					}
				}

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Termino de actualizacion de datos de AFP hacia SQL...");
			}

			return resultado;
		}

		public async Task<Resultado> ActualizacionSALUD()
		{
			Resultado resultado = new Resultado();
			ResultadoExeSqlInsUpd resultadoExeSqlInsUpd = new ResultadoExeSqlInsUpd();
			//RESCATAMOS DATOS DE TABLA DE PAISES DE REX
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde Paso Instituciones SALUD REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var afps = conexion.Query<Institucions>(@"
													      SELECT * FROM REX_INSTITUCION WHERE CLASIFICACION = 'is' ");

				string spName = "SP_REXSQL_INS_UPD_SALUD";

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio actualización de datos de instituciones SALUD hacia SQL...");

				foreach (var afp in afps)
				{

					DynamicParameters dynamicParameters = new DynamicParameters();
					// Adding Input parameters.
					dynamicParameters.Add("@INSTITUCION", afp.Institucion);
					dynamicParameters.Add("@NOMBRE", afp.Nombre);
					// Adding Output parameter.
					dynamicParameters.Add("@CANTIDAD_INS", DbType.Int32, direction: ParameterDirection.Output);
					dynamicParameters.Add("@CANTIDAD_ACT", DbType.Int32, direction: ParameterDirection.Output);
					conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
					resultadoExeSqlInsUpd.CantidadIns = dynamicParameters.Get<int>("@CANTIDAD_INS");
					resultadoExeSqlInsUpd.CantidadUpd = dynamicParameters.Get<int>("@CANTIDAD_ACT");
					if (resultadoExeSqlInsUpd.CantidadIns > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("SALUD creada en base relacional : " + afp.Nombre);
					}
					else if (resultadoExeSqlInsUpd.CantidadUpd > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("SALUD actualizada en base relacional : " + afp.Nombre);
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Error al actualizar o crear SALUD : " + afp.Nombre);
						resultado.Estado = "NO-OK";
					}
				}

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Termino de actualizacion de datos de SALUD hacia SQL...");
			}

			return resultado;
		}

		public async Task<Resultado> ActualizacionLicenciaConducir()
		{
			Resultado resultado = new Resultado();
			ResultadoExeSqlInsUpd resultadoExeSqlInsUpd = new ResultadoExeSqlInsUpd();
			//RESCATAMOS DATOS DE TABLA DE PAISES DE REX
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde Paso Licencias de Conducir REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var licenciasConducir = conexion.Query<CatalogoRex>(@"
											     		            SELECT * FROM REX_CATALOGO WHERE LISTA = 'licenciasConducir' ");

				string spName = "SP_REXSQL_INS_UPD_LICENCIAS_CONDUCIR";

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio actualización de datos de licencias de conducir hacia SQL...");

				foreach (var licenciaConducir in licenciasConducir)
				{

					DynamicParameters dynamicParameters = new DynamicParameters();
					// Adding Input parameters.
					dynamicParameters.Add("@ID", licenciaConducir.Id);
					dynamicParameters.Add("@ITEM", licenciaConducir.Item);
					dynamicParameters.Add("@NOMBRE", licenciaConducir.Nombre);
					if (licenciaConducir.Habilitado)
					{
						dynamicParameters.Add("@VIGENCIA", "S");
					}
					else
					{
						dynamicParameters.Add("@VIGENCIA", "N");
					}
					// Adding Output parameter.
					dynamicParameters.Add("@CANTIDAD_INS", DbType.Int32, direction: ParameterDirection.Output);
					dynamicParameters.Add("@CANTIDAD_ACT", DbType.Int32, direction: ParameterDirection.Output);
					conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
					resultadoExeSqlInsUpd.CantidadIns = dynamicParameters.Get<int>("@CANTIDAD_INS");
					resultadoExeSqlInsUpd.CantidadUpd = dynamicParameters.Get<int>("@CANTIDAD_ACT");
					if (resultadoExeSqlInsUpd.CantidadIns > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Licencia de conducir creada en base relacional : " + licenciaConducir.Nombre);
					}
					else if (resultadoExeSqlInsUpd.CantidadUpd > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Licencia de conducir actualizada en base relacional : " + licenciaConducir.Nombre);
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Error al actualizar o crear licencia de conducir : " + licenciaConducir.Nombre);
						resultado.Estado = "NO-OK";
					}
				}

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Termino de actualizacion de datos de licencias de conducir hacia SQL...");
			}

			return resultado;
		}

		public async Task<Resultado> ActualizacionNivelEstudio()
		{
			Resultado resultado = new Resultado();
			ResultadoExeSqlInsUpd resultadoExeSqlInsUpd = new ResultadoExeSqlInsUpd();
			//RESCATAMOS DATOS DE TABLA DE NIVELES DE ESTUDIO DE REX
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde Paso Nivel Estudio REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var nivelesEstudio = conexion.Query<CatalogoRex>(@"
													             SELECT * FROM REX_CATALOGO WHERE LISTA = 'nivelesEstudio' ");

				string spName = "SP_REXSQL_INS_UPD_NIVEL_ESTUDIO";

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio actualización de datos de niveles de estudio hacia SQL...");

				foreach (var nivelEstudio in nivelesEstudio)
				{

					DynamicParameters dynamicParameters = new DynamicParameters();
					// Adding Input parameters.
					dynamicParameters.Add("@ID", nivelEstudio.Id);
					dynamicParameters.Add("@ITEM", nivelEstudio.Item);
					dynamicParameters.Add("@NOMBRE", nivelEstudio.Nombre);
					if (nivelEstudio.Habilitado)
					{
						dynamicParameters.Add("@VIGENCIA", "S");
					}
					else
					{
						dynamicParameters.Add("@VIGENCIA", "N");
					}
					// Adding Output parameter.
					dynamicParameters.Add("@CANTIDAD_INS", DbType.Int32, direction: ParameterDirection.Output);
					dynamicParameters.Add("@CANTIDAD_ACT", DbType.Int32, direction: ParameterDirection.Output);
					conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
					resultadoExeSqlInsUpd.CantidadIns = dynamicParameters.Get<int>("@CANTIDAD_INS");
					resultadoExeSqlInsUpd.CantidadUpd = dynamicParameters.Get<int>("@CANTIDAD_ACT");
					if (resultadoExeSqlInsUpd.CantidadIns > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Nivel de estudio creado en base relacional : " + nivelEstudio.Nombre);
					}
					else if (resultadoExeSqlInsUpd.CantidadUpd > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Mivel de estudio actualizado en base relacional : " + nivelEstudio.Nombre);
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Error al actualizar o crear nivel de estudio : " + nivelEstudio.Nombre);
						resultado.Estado = "NO-OK";
					}
				}

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Termino de actualizacion de datos de niveles de estudio hacia SQL...");
			}

			return resultado;
		}

		public async Task<Resultado> ActualizacionVinculoFamiliar()
		{
			Resultado resultado = new Resultado();
			ResultadoExeSqlInsUpd resultadoExeSqlInsUpd = new ResultadoExeSqlInsUpd();
			//RESCATAMOS DATOS DE TABLA DE VINCULO FAMILIAR DE REX
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde Paso Vinculo Familiar REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var relacionesFamiliares = conexion.Query<CatalogoRex>(@"
										   							   SELECT * FROM REX_CATALOGO WHERE LISTA = 'relaciones' ");

				string spName = "SP_REXSQL_INS_UPD_RELACION_FAMILIAR";

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio actualización de datos de relaciones familiares hacia SQL...");

				foreach (var relacionFamiliar in relacionesFamiliares)
				{

					DynamicParameters dynamicParameters = new DynamicParameters();
					// Adding Input parameters.
					dynamicParameters.Add("@ID", relacionFamiliar.Id);
					dynamicParameters.Add("@ITEM", relacionFamiliar.Item);
					dynamicParameters.Add("@NOMBRE", relacionFamiliar.Nombre);
					if (relacionFamiliar.Habilitado)
					{
						dynamicParameters.Add("@VIGENCIA", "S");
					}
					else
					{
						dynamicParameters.Add("@VIGENCIA", "N");
					}
					// Adding Output parameter.
					dynamicParameters.Add("@CANTIDAD_INS", DbType.Int32, direction: ParameterDirection.Output);
					dynamicParameters.Add("@CANTIDAD_ACT", DbType.Int32, direction: ParameterDirection.Output);
					conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
					resultadoExeSqlInsUpd.CantidadIns = dynamicParameters.Get<int>("@CANTIDAD_INS");
					resultadoExeSqlInsUpd.CantidadUpd = dynamicParameters.Get<int>("@CANTIDAD_ACT");
					if (resultadoExeSqlInsUpd.CantidadIns > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Relacion familiar creada en base relacional : " + relacionFamiliar.Nombre);
					}
					else if (resultadoExeSqlInsUpd.CantidadUpd > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Relacion familiar actualizada en base relacional : " + relacionFamiliar.Nombre);
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Error al actualizar o crear relacion familiar : " + relacionFamiliar.Nombre);
						resultado.Estado = "NO-OK";
					}
				}

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Termino de actualizacion de datos de relaciones familiares hacia SQL...");
			}

			return resultado;
		}

		public async Task<Resultado> ActualizacionTerminoContrato()
		{
			Resultado resultado = new Resultado();
			ResultadoExeSqlInsUpd resultadoExeSqlInsUpd = new ResultadoExeSqlInsUpd();
			//RESCATAMOS DATOS DE TABLA DE TERMINOS DE CONTRATO DE REX (CATALOGO)
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde Paso términos de contarto REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var terminosContrato = conexion.Query<CatalogoRex>(@"
										   							   SELECT * FROM REX_CATALOGO WHERE LISTA = 'finiquitos' ");

				string spName = "SP_REXSQL_INS_UPD_TERMINO_CONTRATO";

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio actualización de datos de termino de contartos hacia SQL...");

				foreach (var terminoContrato in terminosContrato)
				{

					DynamicParameters dynamicParameters = new DynamicParameters();
					// Adding Input parameters.
					dynamicParameters.Add("@ID", terminoContrato.Id);
					dynamicParameters.Add("@ITEM", terminoContrato.Item);
					dynamicParameters.Add("@NOMBRE", terminoContrato.Nombre);
					if (terminoContrato.Habilitado)
					{
						dynamicParameters.Add("@VIGENCIA", "S");
					}
					else
					{
						dynamicParameters.Add("@VIGENCIA", "N");
					}
					// Adding Output parameter.
					dynamicParameters.Add("@CANTIDAD_INS", DbType.Int32, direction: ParameterDirection.Output);
					dynamicParameters.Add("@CANTIDAD_ACT", DbType.Int32, direction: ParameterDirection.Output);
					conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
					resultadoExeSqlInsUpd.CantidadIns = dynamicParameters.Get<int>("@CANTIDAD_INS");
					resultadoExeSqlInsUpd.CantidadUpd = dynamicParameters.Get<int>("@CANTIDAD_ACT");
					if (resultadoExeSqlInsUpd.CantidadIns > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Término de contrato creado en base relacional : " + terminoContrato.Nombre);
					}
					else if (resultadoExeSqlInsUpd.CantidadUpd > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Término de contrato actualizado en base relacional : " + terminoContrato.Nombre);
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Error al actualizar o crear término de contrato : " + terminoContrato.Nombre);
						resultado.Estado = "NO-OK";
					}
				}

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Termino de actualizacion de datos de términos de contrato hacia SQL...");
			}

			return resultado;
		}

		public async Task<Resultado> ActualizacionTipoContrato()
		{
			Resultado resultado = new Resultado();
			ResultadoExeSqlInsUpd resultadoExeSqlInsUpd = new ResultadoExeSqlInsUpd();
			//RESCATAMOS DATOS DE TABLA DE TIPOS DE CONTRATO DE REX (CONSTANTES)
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde Paso tipos de contrato REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var tiposContrato = conexion.Query<ConstanteRex>(@"
										   							   SELECT * FROM REX_CONSTANTE WHERE LISTA = 'tipoCont' ");

				string spName = "SP_REXSQL_INS_UPD_TIPO_CONTRATO";

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio actualización de datos de termino de contratos hacia SQL...");

				foreach (var tipoContrato in tiposContrato)
				{

					DynamicParameters dynamicParameters = new DynamicParameters();
					// Adding Input parameters.
					dynamicParameters.Add("@ITEM", tipoContrato.Item);
					dynamicParameters.Add("@NOMBRE", tipoContrato.Nombre);
					dynamicParameters.Add("@VIGENCIA", "S");
					// Adding Output parameter.
					dynamicParameters.Add("@CANTIDAD_INS", DbType.Int32, direction: ParameterDirection.Output);
					dynamicParameters.Add("@CANTIDAD_ACT", DbType.Int32, direction: ParameterDirection.Output);
					conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
					resultadoExeSqlInsUpd.CantidadIns = dynamicParameters.Get<int>("@CANTIDAD_INS");
					resultadoExeSqlInsUpd.CantidadUpd = dynamicParameters.Get<int>("@CANTIDAD_ACT");
					if (resultadoExeSqlInsUpd.CantidadIns > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Tipo de contrato creado en base relacional : " + tipoContrato.Nombre);
					}
					else if (resultadoExeSqlInsUpd.CantidadUpd > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Tipo de contrato actualizado en base relacional : " + tipoContrato.Nombre);
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Error al actualizar o crear tipo de contrato : " + tipoContrato.Nombre);
						resultado.Estado = "NO-OK";
					}
				}

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Termino de actualizacion de datos de tipos de contrato hacia SQL...");
			}

			return resultado;
		}

		public async Task<Resultado> ActualizacionEstadoContrato()
		{
			Resultado resultado = new Resultado();
			ResultadoExeSqlInsUpd resultadoExeSqlInsUpd = new ResultadoExeSqlInsUpd();
			//RESCATAMOS DATOS DE TABLA DE ESTADOS DE CONTRATO DE REX (CONSTANTES)
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde Paso estados de contrato REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var estadosContrato = conexion.Query<ConstanteRex>(@"
										   							   SELECT * FROM REX_CONSTANTE WHERE LISTA = 'estado' ");

				string spName = "SP_REXSQL_INS_UPD_ESTADO_CONTRATO";

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio actualización de datos de estado de contratos hacia SQL...");

				foreach (var estadoContrato in estadosContrato)
				{

					DynamicParameters dynamicParameters = new DynamicParameters();
					// Adding Input parameters.
					dynamicParameters.Add("@ITEM", estadoContrato.Item);
					dynamicParameters.Add("@NOMBRE", estadoContrato.Nombre);
					dynamicParameters.Add("@VIGENCIA", "S");
					// Adding Output parameter.
					dynamicParameters.Add("@CANTIDAD_INS", DbType.Int32, direction: ParameterDirection.Output);
					dynamicParameters.Add("@CANTIDAD_ACT", DbType.Int32, direction: ParameterDirection.Output);
					conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
					resultadoExeSqlInsUpd.CantidadIns = dynamicParameters.Get<int>("@CANTIDAD_INS");
					resultadoExeSqlInsUpd.CantidadUpd = dynamicParameters.Get<int>("@CANTIDAD_ACT");
					if (resultadoExeSqlInsUpd.CantidadIns > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Estado de contrato creado en base relac|ional : " + estadoContrato.Nombre);
					}
					else if (resultadoExeSqlInsUpd.CantidadUpd > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Estado de contrato actualizado en base relacional : " + estadoContrato.Nombre);
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Error al actualizar o crear estado de contrato : " + estadoContrato.Nombre);
						resultado.Estado = "NO-OK";
					}
				}

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Termino de actualizacion de datos de estados de contrato hacia SQL...");
			}

			return resultado;
		}

		public async Task<Resultado> ActualizacionProfesion()
		{
			Resultado resultado = new Resultado();
			ResultadoExeSqlInsUpd resultadoExeSqlInsUpd = new ResultadoExeSqlInsUpd();
			//RESCATAMOS DATOS DE TABLA DE PROFESIONES DE REX
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde Paso Profesiones REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var profesiones = conexion.Query<CatalogoRex>(@"
										   				      SELECT * FROM REX_CATALOGO WHERE LISTA = 'profesiones' ");

				string spName = "SP_REXSQL_INS_UPD_PROFESION";

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio actualización de datos de profesiones hacia SQL...");

				foreach (var profesion in profesiones)
				{

					DynamicParameters dynamicParameters = new DynamicParameters();
					// Adding Input parameters.
					dynamicParameters.Add("@ID", profesion.Id);
					dynamicParameters.Add("@ITEM", profesion.Item);
					dynamicParameters.Add("@NOMBRE", profesion.Nombre);
					if (profesion.Habilitado)
					{
						dynamicParameters.Add("@VIGENCIA", "S");
					}
					else
					{
						dynamicParameters.Add("@VIGENCIA", "N");
					}
					// Adding Output parameter.
					dynamicParameters.Add("@CANTIDAD_INS", DbType.Int32, direction: ParameterDirection.Output);
					dynamicParameters.Add("@CANTIDAD_ACT", DbType.Int32, direction: ParameterDirection.Output);
					conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
					resultadoExeSqlInsUpd.CantidadIns = dynamicParameters.Get<int>("@CANTIDAD_INS");
					resultadoExeSqlInsUpd.CantidadUpd = dynamicParameters.Get<int>("@CANTIDAD_ACT");
					if (resultadoExeSqlInsUpd.CantidadIns > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Profesion creada en base relacional : " + profesion.Nombre);
					}
					else if (resultadoExeSqlInsUpd.CantidadUpd > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Profesion actualizada en base relacional : " + profesion.Nombre);
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Error al actualizar o crear profesion : " + profesion.Nombre);
						resultado.Estado = "NO-OK";
					}
				}

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Termino de actualizacion de datos de profesiones hacia SQL...");
			}

			return resultado;
		}

		public async Task<Resultado> ActualizacionNivelOcupacional()
		{
			Resultado resultado = new Resultado();
			ResultadoExeSqlInsUpd resultadoExeSqlInsUpd = new ResultadoExeSqlInsUpd();
			//RESCATAMOS DATOS DE TABLA DE NIVELES OCUPACIONALES DE REX
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde Paso niveles ocupacionales REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var nivelesOcupacionales = conexion.Query<CatalogoRex>(@"
										   							   SELECT * FROM REX_CATALOGO WHERE LISTA = 'nivelOcupacional' ");

				string spName = "SP_REXSQL_INS_UPD_NIVEL_OCUPACIONAL";

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio actualización de datos de niveles ocupacionales hacia SQL...");

				foreach (var nivelOcupacional in nivelesOcupacionales)
				{

					DynamicParameters dynamicParameters = new DynamicParameters();
					// Adding Input parameters.
					dynamicParameters.Add("@ID", nivelOcupacional.Id);
					dynamicParameters.Add("@ITEM", nivelOcupacional.Item);
					dynamicParameters.Add("@NOMBRE", nivelOcupacional.Nombre);
					if (nivelOcupacional.Habilitado)
					{
						dynamicParameters.Add("@VIGENCIA", "S");
					}
					else
					{
						dynamicParameters.Add("@VIGENCIA", "N");
					}
					// Adding Output parameter.
					dynamicParameters.Add("@CANTIDAD_INS", DbType.Int32, direction: ParameterDirection.Output);
					dynamicParameters.Add("@CANTIDAD_ACT", DbType.Int32, direction: ParameterDirection.Output);
					conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
					resultadoExeSqlInsUpd.CantidadIns = dynamicParameters.Get<int>("@CANTIDAD_INS");
					resultadoExeSqlInsUpd.CantidadUpd = dynamicParameters.Get<int>("@CANTIDAD_ACT");
					if (resultadoExeSqlInsUpd.CantidadIns > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Nivel ocupacional creado en base relacional : " + nivelOcupacional.Nombre);
					}
					else if (resultadoExeSqlInsUpd.CantidadUpd > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Nivel ocupacional actualizado en base relacional : " + nivelOcupacional.Nombre);
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Error al actualizar o crear nivel ocupacional : " + nivelOcupacional.Nombre);
						resultado.Estado = "NO-OK";
					}
				}

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Termino de actualizacion de datos de niveles ocupacionales hacia SQL...");
			}

			return resultado;
		}

		public async Task<Resultado> ActualizacionCargoLiquidacion()
		{
			Resultado resultado = new Resultado();
			ResultadoExeSqlInsUpd resultadoExeSqlInsUpd = new ResultadoExeSqlInsUpd();
			//RESCATAMOS DATOS DE TABLA DE CARGOS LIQUIDACION DE REX
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde Paso cargos liquidación REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var cargosLiquidacion = conexion.Query<CatalogoRex>(@"
									   							    SELECT * FROM REX_CATALOGO WHERE LISTA = 'cargoLiq' ");

				string spName = "SP_REXSQL_INS_UPD_CARGO_LIQUIDACION";

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio actualización de datos de cargos liquidación hacia SQL...");

				foreach (var cargoLiquidacion in cargosLiquidacion)
				{

					DynamicParameters dynamicParameters = new DynamicParameters();
					// Adding Input parameters.
					dynamicParameters.Add("@ID", cargoLiquidacion.Id);
					dynamicParameters.Add("@ITEM", cargoLiquidacion.Item);
					dynamicParameters.Add("@NOMBRE", cargoLiquidacion.Nombre);
					if (cargoLiquidacion.Habilitado)
					{
						dynamicParameters.Add("@VIGENCIA", "S");
					}
					else
					{
						dynamicParameters.Add("@VIGENCIA", "N");
					}
					// Adding Output parameter.
					dynamicParameters.Add("@CANTIDAD_INS", DbType.Int32, direction: ParameterDirection.Output);
					dynamicParameters.Add("@CANTIDAD_ACT", DbType.Int32, direction: ParameterDirection.Output);
					conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
					resultadoExeSqlInsUpd.CantidadIns = dynamicParameters.Get<int>("@CANTIDAD_INS");
					resultadoExeSqlInsUpd.CantidadUpd = dynamicParameters.Get<int>("@CANTIDAD_ACT");
					if (resultadoExeSqlInsUpd.CantidadIns > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Cargo liquidación creado en base relacional : " + cargoLiquidacion.Nombre);
					}
					else if (resultadoExeSqlInsUpd.CantidadUpd > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Cargo liquidación actualizado en base relacional : " + cargoLiquidacion.Nombre);
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Error al actualizar o crear cargo liquidación : " + cargoLiquidacion.Nombre);
						resultado.Estado = "NO-OK";
					}
				}

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Termino de actualizacion de datos de cargos liquidacion hacia SQL...");
			}

			return resultado;
		}

		public async Task<Resultado> ActualizacionCargoGenerico()
		{
			Resultado resultado = new Resultado();
			ResultadoExeSqlInsUpd resultadoExeSqlInsUpd = new ResultadoExeSqlInsUpd();
			//RESCATAMOS DATOS DE TABLA DE CARGOS LIQUIDACION DE REX
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde paso cargos genericos REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var cargosGenerico = conexion.Query<CatalogoRex>(@"
									   							 SELECT * FROM REX_CATALOGO WHERE LISTA = 'lta10' ");

				string spName = "SP_REXSQL_INS_UPD_CARGO_GENERICO";

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio actualización de datos de cargos genericos hacia SQL...");

				foreach (var cargoGenerico in cargosGenerico)
				{

					DynamicParameters dynamicParameters = new DynamicParameters();
					// Adding Input parameters.
					dynamicParameters.Add("@ID", cargoGenerico.Id);
					dynamicParameters.Add("@ITEM", cargoGenerico.Item);
					dynamicParameters.Add("@NOMBRE", cargoGenerico.Nombre);
					if (cargoGenerico.Habilitado)
					{
						dynamicParameters.Add("@VIGENCIA", "S");
					}
					else
					{
						dynamicParameters.Add("@VIGENCIA", "N");
					}
					dynamicParameters.Add("@VALOR_A", cargoGenerico.Valora); 


					// Adding Output parameter.
					dynamicParameters.Add("@CANTIDAD_INS", DbType.Int32, direction: ParameterDirection.Output);
					dynamicParameters.Add("@CANTIDAD_ACT", DbType.Int32, direction: ParameterDirection.Output);
					conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
					resultadoExeSqlInsUpd.CantidadIns = dynamicParameters.Get<int>("@CANTIDAD_INS");
					resultadoExeSqlInsUpd.CantidadUpd = dynamicParameters.Get<int>("@CANTIDAD_ACT");
					if (resultadoExeSqlInsUpd.CantidadIns > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Cargo generico creado en base relacional : " + cargoGenerico.Nombre);
					}
					else if (resultadoExeSqlInsUpd.CantidadUpd > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Cargo generico actualizado en base relacional : " + cargoGenerico.Nombre);
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Error al actualizar o crear cargo generico : " + cargoGenerico.Nombre);
						resultado.Estado = "NO-OK";
					}
				}

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Termino de actualizacion de datos de cargos genericos hacia SQL...");
			}

			return resultado;
		}

		public async Task<Resultado> ActualizacionReferencia1()
		{
			Resultado resultado = new Resultado();
			ResultadoExeSqlInsUpd resultadoExeSqlInsUpd = new ResultadoExeSqlInsUpd();
			//RESCATAMOS DATOS DE TABLA DE REFRENCIAS 1 DE REX (CATALOGOS)
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde paso referencias 1 REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var referencias1 = conexion.Query<CatalogoRex>(@"
								  							   SELECT * FROM REX_CATALOGO WHERE LISTA = 'lta9' ");

				string spName = "SP_REXSQL_INS_UPD_REFERENCIA1";

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio actualización de datos de referencias 1 hacia SQL...");

				foreach (var referencia1 in referencias1)
				{

					DynamicParameters dynamicParameters = new DynamicParameters();
					// Adding Input parameters.
					dynamicParameters.Add("@ID", referencia1.Id);
					dynamicParameters.Add("@ITEM", referencia1.Item);
					dynamicParameters.Add("@NOMBRE", referencia1.Nombre);
					if (referencia1.Habilitado)
					{
						dynamicParameters.Add("@VIGENCIA", "S");
					}
					else
					{
						dynamicParameters.Add("@VIGENCIA", "N");
					}
					// Adding Output parameter.
					dynamicParameters.Add("@CANTIDAD_INS", DbType.Int32, direction: ParameterDirection.Output);
					dynamicParameters.Add("@CANTIDAD_ACT", DbType.Int32, direction: ParameterDirection.Output);
					conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
					resultadoExeSqlInsUpd.CantidadIns = dynamicParameters.Get<int>("@CANTIDAD_INS");
					resultadoExeSqlInsUpd.CantidadUpd = dynamicParameters.Get<int>("@CANTIDAD_ACT");
					if (resultadoExeSqlInsUpd.CantidadIns > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Refrencia 1 creada en base relacional : " + referencia1.Nombre);
					}
					else if (resultadoExeSqlInsUpd.CantidadUpd > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Refrencia 1 actualizada en base relacional : " + referencia1.Nombre);
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Error al actualizar o crear refrencia 1 : " + referencia1.Nombre);
						resultado.Estado = "NO-OK";
					}
				}

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Termino de actualizacion de datos de refrencias 1 hacia SQL...");
			}

			return resultado;
		}

		public async Task<Resultado> ActualizacionReferencia2()
		{
			Resultado resultado = new Resultado();
			ResultadoExeSqlInsUpd resultadoExeSqlInsUpd = new ResultadoExeSqlInsUpd();
			//RESCATAMOS DATOS DE TABLA DE REFRENCIAS 2 DE REX (CATALOGOS)
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde paso referencias 2 REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var referencias2 = conexion.Query<CatalogoRex>(@"
								  							   SELECT * FROM REX_CATALOGO WHERE LISTA = 'lta4' ");

				string spName = "SP_REXSQL_INS_UPD_REFERENCIA2";

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio actualización de datos de referencias 2 hacia SQL...");

				foreach (var referencia2 in referencias2)
				{

					DynamicParameters dynamicParameters = new DynamicParameters();
					// Adding Input parameters.
					dynamicParameters.Add("@ID", referencia2.Id);
					dynamicParameters.Add("@ITEM", referencia2.Item);
					dynamicParameters.Add("@NOMBRE", referencia2.Nombre);
					if (referencia2.Habilitado)
					{
						dynamicParameters.Add("@VIGENCIA", "S");
					}
					else
					{
						dynamicParameters.Add("@VIGENCIA", "N");
					}
					// Adding Output parameter.
					dynamicParameters.Add("@CANTIDAD_INS", DbType.Int32, direction: ParameterDirection.Output);
					dynamicParameters.Add("@CANTIDAD_ACT", DbType.Int32, direction: ParameterDirection.Output);
					conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
					resultadoExeSqlInsUpd.CantidadIns = dynamicParameters.Get<int>("@CANTIDAD_INS");
					resultadoExeSqlInsUpd.CantidadUpd = dynamicParameters.Get<int>("@CANTIDAD_ACT");
					if (resultadoExeSqlInsUpd.CantidadIns > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Refrencia 2 creada en base relacional : " + referencia2.Nombre);
					}
					else if (resultadoExeSqlInsUpd.CantidadUpd > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Refrencia 2 actualizada en base relacional : " + referencia2.Nombre);
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Error al actualizar o crear refrencia 2 : " + referencia2.Nombre);
						resultado.Estado = "NO-OK";
					}
				}

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Termino de actualizacion de datos de refrencias 2 hacia SQL...");
			}

			return resultado;
		}

		public async Task<Resultado> ActualizacionCentroCosto()
		{
			Resultado resultado = new Resultado();
			ResultadoExeSqlInsUpd resultadoExeSqlInsUpd = new ResultadoExeSqlInsUpd();
			//RESCATAMOS DATOS DE TABLA DE CENTROS DE COSTO DE REX
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde Paso centros de costo REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var centrosCosto = conexion.Query<CatalogoRex>(@"
										   					   SELECT * FROM REX_CENTRO_COSTO ");

				string spName = "SP_REXSQL_INS_UPD_CENTRO_COSTO";

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio actualización de datos de centros de costo hacia SQL...");

				foreach (var centroCosto in centrosCosto)
				{

					DynamicParameters dynamicParameters = new DynamicParameters();
					// Adding Input parameters.
					dynamicParameters.Add("@ID", centroCosto.Id);
					dynamicParameters.Add("@FECHA_CREACION", centroCosto.Fecha_Creacion);
					dynamicParameters.Add("@FECHA_MODIFICACION", centroCosto.Fecha_Modificacion);
					dynamicParameters.Add("@ITEM", centroCosto.Item);
					dynamicParameters.Add("@NOMBRE", centroCosto.Nombre);
					dynamicParameters.Add("@DATOADIC", centroCosto.DatoAdic);
					if (centroCosto.Habilitado)
					{
						dynamicParameters.Add("@VIGENCIA", "S");
					}
					else
					{
						dynamicParameters.Add("@VIGENCIA", "N");
					}
					// Adding Output parameter.
					dynamicParameters.Add("@CANTIDAD_INS", DbType.Int32, direction: ParameterDirection.Output);
					dynamicParameters.Add("@CANTIDAD_ACT", DbType.Int32, direction: ParameterDirection.Output);
					conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
					resultadoExeSqlInsUpd.CantidadIns = dynamicParameters.Get<int>("@CANTIDAD_INS");
					resultadoExeSqlInsUpd.CantidadUpd = dynamicParameters.Get<int>("@CANTIDAD_ACT");
					if (resultadoExeSqlInsUpd.CantidadIns > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Centro de costo creado en base relacional : " + centroCosto.Nombre);
					}
					else if (resultadoExeSqlInsUpd.CantidadUpd > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Centro de costo actualizado en base relacional : " + centroCosto.Nombre);
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Error al actualizar o crear centro de costo : " + centroCosto.Nombre);
						resultado.Estado = "NO-OK";
					}
				}

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Termino de actualizacion de datos de centros de costo hacia SQL...");
			}

			return resultado;
		}

		public async Task<Resultado> ActualizacionColaborador()
		{
			Resultado resultado = new Resultado();
			ResultadoExeSqlInsUpd resultadoExeSqlInsUpd = new ResultadoExeSqlInsUpd();
			//RESCATAMOS DATOS DE TABLA DE COLABORADORES DE REX
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde paso colaboradores REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var colaboradores = conexion.Query<Colaborador>(@"
										   					    SELECT * FROM REX_COLABORADOR ");

				string spName = "SP_REXSQL_INS_UPD_COLABORADOR";

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio actualización de datos de colaboradores hacia SQL...");

				foreach (var colaborador in colaboradores)
				{

					DynamicParameters dynamicParameters = new DynamicParameters();
					// Adding Input parameters.
					dynamicParameters.Add("@EMPLEADO", colaborador.Empleado.Trim().Substring(0, colaborador.Empleado.Trim().Length -2));
					dynamicParameters.Add("@EMPLEADO_DV", colaborador.Empleado.Trim().Substring(colaborador.Empleado.Trim().Length -1 , 1));
					dynamicParameters.Add("@FORMAPAGO", colaborador.FormaPago);
					dynamicParameters.Add("@BANCO", colaborador.Banco);
					dynamicParameters.Add("@CUANTABANCO", colaborador.CuentaBanc);
					dynamicParameters.Add("@SUCURSAL", colaborador.Sucursal);
					dynamicParameters.Add("@FECHACAMBIO", colaborador.FechaCambio);
					dynamicParameters.Add("@NOMBRE", colaborador.Nombre);
					dynamicParameters.Add("@APELLIDOPATE", colaborador.ApellidoPate);
					dynamicParameters.Add("@APELLIDOMATE", colaborador.ApellidoMate);

					dynamicParameters.Add("@SEXO", colaborador.Sexo);
					dynamicParameters.Add("@FECHANACI", colaborador.FechaNaci);
					dynamicParameters.Add("@ESTADOCIVI", colaborador.EstadoCivi);
					dynamicParameters.Add("@NACION", colaborador.Nacion);
					dynamicParameters.Add("@DIRECCION", colaborador.Direccion);
					dynamicParameters.Add("@CIUDAD", colaborador.Ciudad);
					dynamicParameters.Add("@NUMEROFONO", colaborador.NumeroFono);
					dynamicParameters.Add("@EMAIL", colaborador.Email);
					dynamicParameters.Add("@CONTRATOACTI", colaborador.ContratoActi);
					dynamicParameters.Add("@SUPERVISA", colaborador.Supervisa);

					dynamicParameters.Add("@AFP", colaborador.Afp);
					dynamicParameters.Add("@AFPCOTI", colaborador.AfpCoti);
					dynamicParameters.Add("@AFPFECHAAFILIZACION", colaborador.Afp_Fecha_Afiliacion);
					dynamicParameters.Add("@JUBILADO", colaborador.Jubilado);
					dynamicParameters.Add("@EXPATRIADO", colaborador.Expatriado);
					dynamicParameters.Add("@SISTEMAPENS", colaborador.SistemaPens);
					dynamicParameters.Add("@ISAPRE", colaborador.Isapre);
					dynamicParameters.Add("@ISAPRECOTI", colaborador.IsapreCoti);
					dynamicParameters.Add("@ISAPREPESO", colaborador.IsaprePeso);
					dynamicParameters.Add("@ISAPREMONE", colaborador.IsapreMone);

					dynamicParameters.Add("@ISAPREFUN", colaborador.IsapreFun);
					dynamicParameters.Add("@TRAMOAF", colaborador.TramoAf);
					dynamicParameters.Add("@CODIGOINTER", colaborador.CodigoInter);
					dynamicParameters.Add("@CODIGOINTER2", colaborador.CodigoInte2);
					dynamicParameters.Add("@AGRUPACION", colaborador.Agrupacion);
					dynamicParameters.Add("@TALLAROPA", colaborador.TallaRopa);
					dynamicParameters.Add("@TALLAZAPATO", colaborador.TallaZapato);
					dynamicParameters.Add("@TALLAPANTALON", colaborador.Talla_Pantalon);
					dynamicParameters.Add("@ESTUDIOS", colaborador.Estudios);
					dynamicParameters.Add("@NUMEROFONO2", colaborador.NumeroFono2);

					dynamicParameters.Add("@EMPRESA", colaborador.Empresa);
					dynamicParameters.Add("@SITUACION", colaborador.Situacion);
					dynamicParameters.Add("@NOTAS", colaborador.Notas);
					dynamicParameters.Add("@EMAILPERSONAL", colaborador.EmailPersonal);
					dynamicParameters.Add("@NIVELESTUDIOS", colaborador.NivelEstudios);
					dynamicParameters.Add("@PROFESION", colaborador.Profesion);
					dynamicParameters.Add("@LICENCIACONDUCIR", colaborador.LicenciaConducir);
					dynamicParameters.Add("@FECHACREACION", colaborador.FechaCreacion);
					dynamicParameters.Add("@ORIGEN", colaborador.Origen);
					dynamicParameters.Add("@COTIZAPREVISIONSALUD", colaborador.Cotiza_Prevision_Salud);

					dynamicParameters.Add("@REGISTRADODEC", colaborador.Registrado_Dec);
					dynamicParameters.Add("@ESPRIVADO", colaborador.Es_Privado);
					dynamicParameters.Add("@ESSOLOAPROBADOR", colaborador.Es_Solo_Aprovador);
					dynamicParameters.Add("@ESRECONTRATABLE", colaborador.Es_Recontratable);
					dynamicParameters.Add("@TIPODOCUMENTO", colaborador.Tipo_Documento);
					dynamicParameters.Add("@NOMBRECONTACTOEMERGENCIA", colaborador.Nombre_Contacto_Emergencia);
					dynamicParameters.Add("@VINCULOCONTACTOEMERGENCIA", colaborador.Vinculo_Contacto_Emergencia);
					dynamicParameters.Add("@TELEFONOCONTACTOEMERGENCIA", colaborador.Telefono_Contacto_Emergencia);
					dynamicParameters.Add("@TELEFONO2CONTACTOEMERGENCIA", colaborador.Telefono2_Contacto_Emergencia);
					dynamicParameters.Add("@NIVELOCUPACIONAL", colaborador.Nivel_Ocupacional);

					// Adding Output parameter.
					dynamicParameters.Add("@CANTIDAD_INS", DbType.Int32, direction: ParameterDirection.Output);
					dynamicParameters.Add("@CANTIDAD_ACT", DbType.Int32, direction: ParameterDirection.Output);
					conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
					resultadoExeSqlInsUpd.CantidadIns = dynamicParameters.Get<int>("@CANTIDAD_INS");
					resultadoExeSqlInsUpd.CantidadUpd = dynamicParameters.Get<int>("@CANTIDAD_ACT");
					if (resultadoExeSqlInsUpd.CantidadIns > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Colaborador creado en base relacional : " + colaborador.Empleado);
					}
					else if (resultadoExeSqlInsUpd.CantidadUpd > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Colaborador actualizado en base relacional : " + colaborador.Empleado);
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Error al actualizar o crear colaborador : " + colaborador.Empleado);
						resultado.Estado = "NO-OK";
					}
				}

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Termino de actualizacion de datos de colaboradores hacia SQL...");
			}

			return resultado;
		}

		public async Task<Resultado> ActualizacionContratoColaborador()
		{
			Resultado resultado = new Resultado();
			ResultadoExeSqlInsUpd resultadoExeSqlInsUpd = new ResultadoExeSqlInsUpd();
			//RESCATAMOS DATOS DE TABLA DE CONTRATOS DE REX
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("Inicio carga de datos desde paso contratos REX...");
			resultado.Estado = "OK";
			using (var conexion = new SqlConnection(_connectionString))
			{
				var contratos = conexion.Query<ContratoRex>(@"
										   				 SELECT * FROM REX_CONTRATO ");

				string spName = "SP_REXSQL_INS_UPD_CONTRATO_COLABORADOR";

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Inicio actualización de datos de contratos hacia SQL...");

				foreach (var contrato in contratos)
				{

					DynamicParameters dynamicParameters = new DynamicParameters();
					// Adding Input parameters.
					dynamicParameters.Add("@RHCON_FECHA_INICIO", contrato.FechaInic);
					dynamicParameters.Add("@RHCON_FECHA_TERMINO", contrato.FechaTerm);
					dynamicParameters.Add("@CENTROCOST", contrato.CentroCost);
					dynamicParameters.Add("@CARGO_LIQ", contrato.Cargo);
					dynamicParameters.Add("@CAUSAL_TERMINO", contrato.Causal);
					dynamicParameters.Add("@EMPLEADO", contrato.Empleado.Trim().Substring(0,contrato.Empleado.Trim().Length - 2));
					dynamicParameters.Add("@EMPLEADO_DV", contrato.Empleado.Trim().Substring(contrato.Empleado.Trim().Length - 1, 1));
					dynamicParameters.Add("@CARGO_GEN", contrato.Lta10);
					dynamicParameters.Add("@REFERENCIA1", contrato.Lta09);
					dynamicParameters.Add("@REFERENCIA2", contrato.Lta04);
					dynamicParameters.Add("@RHCON_FECHA_CREACION", contrato.Fecha_creacion);

					dynamicParameters.Add("@RHCON_FECHA_MODIFICACION", contrato.Fecha_modificacion);
					dynamicParameters.Add("@RHCON_SUELDO_BASE", contrato.SueldoBase);
					dynamicParameters.Add("@RHCON_NOMBRE", contrato.Nombre);
					dynamicParameters.Add("@RHCON_CONTRATO_NUMERO", contrato.Contrato);
					dynamicParameters.Add("@TIPO_CONTRATO", contrato.TipoCont);
					dynamicParameters.Add("@ESTADO_CONTRATO", contrato.Estado);
					
					// Adding Output parameter.
					dynamicParameters.Add("@CANTIDAD_INS", DbType.Int32, direction: ParameterDirection.Output);
					dynamicParameters.Add("@CANTIDAD_ACT", DbType.Int32, direction: ParameterDirection.Output);
					conexion.Execute(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
					resultadoExeSqlInsUpd.CantidadIns = dynamicParameters.Get<int>("@CANTIDAD_INS");
					resultadoExeSqlInsUpd.CantidadUpd = dynamicParameters.Get<int>("@CANTIDAD_ACT");
					if (resultadoExeSqlInsUpd.CantidadIns > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Contrato de colaborador creado en base relacional : " + contrato.Empleado + " contrato Id: " + contrato.Id.ToString());
					}
					else if (resultadoExeSqlInsUpd.CantidadUpd > 0)
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Contrato de colaborador actualizado en base relacional : " + contrato.Empleado + " contrato Id: " + contrato.Id.ToString());
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Error al actualizar o crear contrato de colaborador : " + contrato.Empleado + " contrato Id: " + contrato.Id.ToString());
						resultado.Estado = "NO-OK";
					}
				}

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Termino de actualizacion de datos de contratos de colaboradores hacia SQL...");
			}

			return resultado;
		}

	}
}
