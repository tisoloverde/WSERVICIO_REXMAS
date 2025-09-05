using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace REX_Consumer_WorkerService.Models
{
	public class LicenciaMedica
	{
		public string Id { get; set; } = string.Empty;
		public List<int> Contratos { get; set; } = new List<int>();
		public decimal Contrato { get; set; } = 0;
		public string Fecha_Ingreso { get; set; } = string.Empty;
		public string Fecha_Inicio { get; set; } = string.Empty;
		public string Fecha_Termino { get; set; } = string.Empty;
		public int Dias { get; set; } = 0;
		public string Tipo_Medio_Dia { get; set; } = string.Empty;
		public string Tipo_Licencia { get; set; } = string.Empty;
		public string Numero_Licencia { get; set; } = string.Empty;
		public string Tipo_Ingreso { get; set; } = string.Empty;
		public string Fecha_Calculo { get; set; } = string.Empty;
		public string Fecha_Aplicacion { get; set; } = string.Empty;
		public string Licencia_Anterior { get; set; } = string.Empty;
		public string Fecha_Licencia_Inicial { get; set; } = string.Empty;
		public int Total_Dias_Continuacion { get; set; } = 0;
		public string Descripcion { get; set; } = string.Empty;
		public string Subtipo_Licencia { get; set; } = string.Empty;
		public string Tipo_Licencia_Inicial { get; set; } = string.Empty;
		public string Medico_Tratante { get; set; } = string.Empty;
		public string Identificador_Medico_Tratante { get; set; } = string.Empty;
		public string Especialidad_Medico { get; set; } = string.Empty;
		public string Nombre_Especialidad_Medico { get; set; } = string.Empty;
		public int Dias_A_Pagar { get; set; } = 0;
		public bool No_Rebaja { get; set; } = false;

	}
}
