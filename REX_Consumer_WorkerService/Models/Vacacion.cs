using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REX_Consumer_WorkerService.Models
{
	public class Vacacion
	{
		public string? Id { get; set; }
		public string? Empleado { get; set; }
		public string? Contrato { get; set; }
	    public string? TipoMovil { get; set; }
		public string? Calificacion { get; set; }
		public string? Dias { get; set; }
		public string? FechaInic { get; set; }
		public string? FechaTerm { get; set; }
		public string? FechaRetorno { get; set; }
		public string? HoraInicio { get; set; }
		public string? Motivo { get; set; }
		public string? Tipo_Medio_Dia { get; set; }
		public string? Firmado { get; set; }
		public string? Valor_Dia_Progresivo { get; set; }
		public string? Fichero_Id { get; set; } 
	}
}
