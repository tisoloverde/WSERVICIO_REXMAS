using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REX_Consumer_WorkerService.Models
{
	public class CentroCosto
	{
		public string Id { get; set; } = string.Empty;
		public string Fecha_Creacion { get; set; } = string.Empty;
		public string Fecha_Modificacion { get; set; } = string.Empty;
		public string Item { get; set; } = string.Empty; 
		public string Nombre { get; set; } = string.Empty;
		public decimal Valora { get; set; } = 0;
		public decimal Valorb { get; set; } = 0;
		public decimal Valorc { get; set; } = 0;
		public string DatoAdic { get; set; } = string.Empty;
		public string Lista { get; set; } = string.Empty;
		public Boolean Habilitado { get; set; } = false;
		public Boolean Reservado { get; set; } = false;

	}
}
