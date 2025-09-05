using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REX_Consumer_WorkerService.Models
{
	public class Permiso
	{
		public string Id { get; set; } = string.Empty;
		public List<int> Contratos { get; set; } = new List<int>();
		public decimal Contrato { get; set; } = 0;
		public string Fecha_Ingreso { get; set; } = string.Empty;
		public string Fecha_Inicio { get; set; } = string.Empty;
		public string Fecha_Termino { get; set; } = string.Empty;
		public int Dias { get; set; } = 0;
		public string Tipo { get; set; } = string.Empty;
		public string Tipo_Nombre { get; set; } = string.Empty;
		public string Subtipo_Permiso { get; set; } = string.Empty;
		public string Subtipo_Permiso_Nombre { get; set; } = string.Empty;
		public Boolean Goce_Sueldo { get; set; } = false;
		public string Descripcion { get; set; } = string.Empty;
		public string Fecha_Aplicacion{ get; set; } = string.Empty;
		public string Tipo_Medio_Dia { get; set; } = string.Empty;
		
	}
}
