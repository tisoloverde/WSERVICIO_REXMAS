using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace REX_Consumer_WorkerService.Models
{
	public class Cargo
	{
		public string? Id { get; set; } 
		public string? Fecha_Creacion { get; set; }
		public string? Fecha_Modificacion { get; set; }
		public string? Item { get; set; }
		public string? Nombre { get; set; }
		public string? ValorA { get; set; }
		public string? ValorB { get; set; }
		public string? ValorC { get; set; }
		public string? DatoAdic { get; set; }

	}
}
