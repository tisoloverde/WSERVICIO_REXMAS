using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REX_Consumer_WorkerService.Models
{
	public class ContratoRex
	{
		public decimal Id { get; set; }
		public string FechaInic { get; set; } = string.Empty;
		public string FechaCambInde { get; set; } = string.Empty;
		public string VacacionesInic { get; set; } = string.Empty;
		public string ProgresivosInic { get; set; } = string.Empty;
		public string FechaTerm { get; set; } = string.Empty;
		public string FechaCesa { get; set; } = string.Empty;
		public decimal ProgresivosReco { get; set; }
		public string Cargo { get; set; } = string.Empty;
		public string Area { get; set; } = string.Empty;
		public string Area_id { get; set; } = string.Empty;
		public string Jefatura_directa_id { get; set; } = string.Empty;
		public string Jefatura_indirecta_id { get; set; } = string.Empty;
		public string Maneja_trato { get; set; } = string.Empty;
		public decimal SueldoBase { get; set; }
		public string FechaCambio { get; set; } = string.Empty;
		public decimal Cargo_id { get; set; }	
		public string Turno { get; set; } = string.Empty;
		public string Cotizacion { get; set; } = string.Empty;	
		public string Fecha_creacion { get; set; } = string.Empty;
		public string Fecha_modificacion { get; set; } = string.Empty;
		public string Empresa { get; set; } = string.Empty;
		public string Nombre { get; set; } = string.Empty;
		public string Estado { get; set; } = string.Empty;
		public string TipoCont { get; set; } = string.Empty;
		public Boolean Afecto_a_trato { get; set; }
		public string Causal { get; set; } = string.Empty;
		public string CentroCost { get; set; } = string.Empty;
		public string Centro_distribucion { get; set; } = string.Empty;
		public string Sede { get; set; } = string.Empty;
		public string PlantillaGrup { get; set; } = string.Empty;
        public Boolean TrabajoPesa { get; set; }
		public string TrabajoPesaPorc { get; set; } = string.Empty;
		public string Sindicato { get; set; } = string.Empty;
		public Boolean JornadaParc { get; set;  }
		public string Pacto_sueldo_unidad { get; set; } = string.Empty;
		public string Pacto_sueldo_valor { get; set; } = string.Empty;
		public string HorasSema { get; set; } = string.Empty;
		public string DistribucionJorn { get; set; } = string.Empty;
		public string Modalidad_contrato { get; set; } = string.Empty;
		public Boolean Permite_ausencias_inhabiles { get; set; }
		public Boolean SeguroCesa { get; set; }
		public string Agrupacion { get; set; } = string.Empty;
		public string CategoriaIne { get; set; } = string.Empty;
		public string Pauta { get; set; } = string.Empty;
		public string Supervisor { get; set; } = string.Empty;
		public Boolean SueldoPatr { get; set; }
		public string DetalleCont { get; set; } = string.Empty;
		public string Lta01 { get; set; } = string.Empty;
		public string Lta02 { get; set; } = string.Empty;
		public string Lta03 { get; set; } = string.Empty;
		public string Lta04 { get; set; } = string.Empty;
		public string Lta05 { get; set; } = string.Empty;
		public string Lta06 { get; set; } = string.Empty;
		public string Lta07 { get; set; } = string.Empty;
		public string Lta08 { get; set; } = string.Empty;
		public string Lta09 { get; set; } = string.Empty;
		public string Lta10 { get; set; } = string.Empty;
		public string Lta11 { get; set; } = string.Empty;
		public string Lta12 { get; set; } = string.Empty;
		public string Lta13 { get; set; } = string.Empty;
		public string Lta14 { get; set; } = string.Empty;
		public string Lta15 { get; set; } = string.Empty;
		public string Lta16 { get; set; } = string.Empty;
		public string Lta17 { get; set; } = string.Empty;
		public string Lta18 { get; set; } = string.Empty;
		public string Lta19 { get; set; } = string.Empty;
		public string Lta20 { get; set; } = string.Empty;
		public string Zona_extrema { get; set; } = string.Empty;
		public string Fca01 { get; set; } = string.Empty;
		public string Fca02 { get; set; } = string.Empty;
		public string Cpa01 { get; set; } = string.Empty;
		public string Cpa02 { get; set; } = string.Empty;
		public string NivelSence { get; set; } = string.Empty;
		public string FactorSence { get; set; } = string.Empty;
		public string Identificador_externo { get; set; } = string.Empty;
		public string Idarchivo { get; set; } = string.Empty;
		public Boolean Descansa_domingos { get; set; }
		public string Caja_compensacion { get; set; } = string.Empty;
		public string Forma_pago_contrato { get; set; } = string.Empty;
		public string Mes_reinicio_administrativos { get; set; } = string.Empty;
		public string Direccion_laboral { get; set; } = string.Empty;
		public Boolean Es_reemplazo { get; set; }
		public Boolean Tiene_subsidio_licencia { get; set; }
		public string Fecha_renovacion { get; set; } = string.Empty;
		public string Fecha_renovacion_2 { get; set; } = string.Empty;
		public Boolean Utiliza_asistencia { get; set; }
		public string Ultimo_proceso_reajuste { get; set; } = string.Empty;
		public string Acceso_liquidacion_portal { get; set; } = string.Empty;
		public string Origen_instrumento_colectivo { get; set; } = string.Empty;
		public string Identificador_instrumento { get; set; } = string.Empty;
		public string Reconocimiento_antiguedad { get; set; } = string.Empty;
		public string Empleado { get; set; } = string.Empty;
		public string Contrato { get; set; } = string.Empty;
		public Boolean Borrador { get; set; }
		public string Jefatura_directa { get; set; } = string.Empty;
		public string Jefatura_indirecta { get; set; } = string.Empty;

	}
}
