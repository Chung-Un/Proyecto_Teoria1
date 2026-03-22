namespace PresupuestoPersonal.Models
{
    public class Categoria
    {
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
        public string TipoCategoria { get; set; }
        public string NombreIcono { get; set; }
        public string ColorHexadecimal { get; set; }
        public int OrdenPresentacion { get; set; }
    }
}