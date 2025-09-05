using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REX_Consumer_WorkerService.Models
{
	internal class MiConfiguracion
	{
		public string UsuarioLogin { get; set; } = string.Empty;
		public string PasswordLogin { get; set; } = string.Empty;
		public string UrlBase { get; set; } = string.Empty;
		public string TodosLosContratos { get; set; } = string.Empty;
		public DateTime FechaActual { get; set; }
		public DateTime FechaDiaAnterior { get; set; }
		public string AplicaConstantes { get; set; } = string.Empty;
		public string FechaCorteColaborador { get; set; } = string.Empty;
		public string FechaInicioVacacion { get; set; } = string.Empty;
		public string FechaInicioLicenciaMedica { get; set; } = string.Empty;
		public string FechaInicioPermisos { get; set; } = string.Empty;
		public string CatalogoBanco { get; set; } = string.Empty;
		public string CatalogoProfesion { get; set; } = string.Empty;
		public string CatalogoFormaPago { get; set; } = string.Empty;
		public string CatalogoCentroCosto { get; set; } = string.Empty;
		public string CatalogoCargo { get; set; } = string.Empty;
		public string CatalogoLicenciaConducir { get; set; } = string.Empty;
		public string CatalogoNivelEstudio { get; set; } = string.Empty;
		public string CatalogoNivelOcupacional { get; set; } = string.Empty;
		public string CatalogoRelaciones {  get; set; } = string.Empty;
		public string CatalogoCausalesTerminoContrato { get; set; } = string.Empty;
		public string CatalogoCargoGenericoUnificado { get; set; } = string.Empty;
		public string CatalogoReferencia1 { get; set; } = string.Empty;
		public string CatalogoReferencia2 { get; set; } = string.Empty;
		public string CatalogoFeriados { get; set; } = string.Empty;
		public string CatalogoFeriadosMovi { get; set; } = string.Empty;
		public string ConstanteTipoContrato { get; set; } = string.Empty;
		public string ConstanteEstadoContrato { get; set; } = string.Empty;

	}
}
