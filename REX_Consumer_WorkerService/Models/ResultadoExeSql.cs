using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REX_Consumer_WorkerService.Models
{
	public class ResultadoExeSql
	{
		public decimal Id { get; set; }
		public int Cantidad { get; set; }
		public int Status { get; set; }
		public string Estado { get; set; }
		public string? Mensaje { get; set; }
	}
}
