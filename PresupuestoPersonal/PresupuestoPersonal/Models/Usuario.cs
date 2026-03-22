
namespace PresupuestoPersonal.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Password { get; set; }
        public string CorreoElectronico { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public DateTime FechaIngreso { get; set; }
        public decimal SalarioMensualBase { get; set; }
        public bool EstadoUsuario { get; set; }
    }
}