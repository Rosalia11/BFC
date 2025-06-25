namespace BFC.Web.Data.Dtos
{
    public class RecestasDtos
    {
        public int Id_Recetas { get; set; }
        public string Nombre { get; set; } = null!;
        public string Categoria { get; set; } = null!;
        public string Tiempo_preparacion { get; set; } = null!;
        public string Imagen { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string Ingredientes { get; set; } = null!;
        public string Preparacion { get; set; } = null!;
    }
}
