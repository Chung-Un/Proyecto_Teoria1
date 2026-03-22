namespace PresupuestoPersonal.Models
{
    public class Subcategoria
    {
        public int IdSubcategoria { get; set; }
        public int IdCategoria { get; set; }
        public string NombreSubcategoria { get; set; }
        public bool EstadoSubcategoria { get; set; }
        public bool SubcategoriaPorDefecto { get; set; }
    }
}