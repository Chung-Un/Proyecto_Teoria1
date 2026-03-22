namespace PresupuestoPersonal.Models
{
    public class PresupuestoDetalle
    {
        public int IdDetalle { get; set; }
        public int IdPresupuesto { get; set; }
        public int IdSubcategoria { get; set; }
        public decimal MontoMensual { get; set; }
        public string Observaciones { get; set; }
    }
}