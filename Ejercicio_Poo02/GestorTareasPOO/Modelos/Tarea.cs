namespace GestorTareasPOO.Modelos
{
    public class Tarea : IExportable
    {
        private static int contador = 1;
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public Prioridad Prioridad { get; set; }

        public Categoria Categoria { get; set; }

        public bool Completada { get; set; }

        public DateTime FechaCreacion { get; set; }

        public Tarea()
        {
            if (Id == 0)
            {
                Id = contador++;
            }

            if (FechaCreacion == DateTime.MinValue)
            {
                FechaCreacion = DateTime.Now;
            }
        }

        public virtual void MostrarInfo()
        {
            Console.WriteLine($"ID: {Id}");
            Console.WriteLine($"Título: {Titulo}");
            Console.WriteLine($"Descripción: {Descripcion}");
            Console.WriteLine($"Prioridad: {Prioridad}");
            Console.WriteLine($"Categoría: {Categoria?.Nombre}");
            Console.WriteLine($"Completada: {Completada}");
            Console.WriteLine($"Fecha de creación: {FechaCreacion}");
        }

        public string Exportar()
        {
            return $"{Id}|{Titulo}|{Prioridad}|{Completada}";
        }

        public void RestaurarDatos(int id, DateTime fechaCreacion)
        {
            Id = id;
            FechaCreacion = fechaCreacion;

            if (contador <= id)
            {
                contador = id + 1;
            }
        }
    }
}