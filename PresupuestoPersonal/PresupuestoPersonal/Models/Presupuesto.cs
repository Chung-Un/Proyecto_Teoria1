namespace PresupuestoPersonal.Models
{
    public class Presupuesto
    {
        public int IdPresupuesto { get; set; }
        public int IdUsuario { get; set; }
        public string NombrePresupuesto { get; set; }
        public short AnioInicio { get; set; }
        public byte MesInicio { get; set; }
        public short AnioFin { get; set; }
        public byte MesFin { get; set; }
        public decimal TotalIngresosPlanificados { get; set; }
        public decimal TotalGastosPlanificados { get; set; }
        public decimal TotalAhorroPlanificado { get; set; }
        public DateTime FechaYHoraCreacion { get; set; }
        public string EstadoPresupuesto { get; set; }
    }
}
