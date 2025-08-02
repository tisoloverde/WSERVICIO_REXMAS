using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REX_Consumer_WorkerService.Models
{
	public class ListaContratosRex
	{
		public decimal Id { get; set; }
		public string Empresa { get; set; } = string.Empty;
		public string Nombre { get; set; } = string.Empty;
		public string Estado { get; set; } = string.Empty;	
		public string Contrato { get; set; } = string.Empty;
	}
}
