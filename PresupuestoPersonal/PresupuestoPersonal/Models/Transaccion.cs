namespace PresupuestoPersonal.Models
{
    public class Transaccion
    {
        public int IdTransaccion { get; set; }
        public int IdUsuario { get; set; }
        public int IdDetalle { get; set; }
        public short AnioTransaccion { get; set; }
        public byte MesTransaccion { get; set; }
        public int IdSubcategoria { get; set; }
        public string TipoTransaccion { get; set; }
        public string DescripcionMovimiento { get; set; }
        public decimal MontoTransaccion { get; set; }
        public DateOnly FechaTransaccion { get; set; }
        public string MetodoPago { get; set; }
        public int? NumeroFactura { get; set; }      // nullable, es opcional
        public string Observaciones { get; set; }
        public DateTime FechaYHoraRegistro { get; set; }
    }
}