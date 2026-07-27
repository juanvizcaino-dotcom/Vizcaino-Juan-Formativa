namespace GestorTareasPOO.Persistencia
{
    public class TareaDTO
    {
        public string Tipo { get; set; }

        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public string Prioridad { get; set; }

        public bool Completada { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaVencimiento { get; set; }

        public string NombreCategoria { get; set; }

        public string ColorCategoria { get; set; }

        public string DescripcionCategoria { get; set; }
    }
}