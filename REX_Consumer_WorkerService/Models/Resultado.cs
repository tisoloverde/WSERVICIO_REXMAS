using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REX_Consumer_WorkerService.Models
{
	public class Resultado
	{
		public int Status { get; set; }
		public string Estado { get; set; } = string.Empty;
		public string Mensaje { get; set; } = string.Empty;

	}
}
