namespace PresupuestoPersonal.Models
{
    public class ObligacionFija
    {
        public int IdObligacion { get; set; }
        public int IdSubcategoria { get; set; }
        public string NombreObligacion { get; set; }
        public decimal MontoMensual { get; set; }
        public byte DiaVencimiento { get; set; }
        public bool EstadoVigente { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }  // nullable porque puede ser indefinida
    }
}

