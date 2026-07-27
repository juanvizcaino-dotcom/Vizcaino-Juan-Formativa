using System.Text.Json;
using GestorTareasPOO.Modelos;
using GestorTareasPOO.Persistencia;

namespace GestorTareasPOO.Servicios
{
    public class GestorTareas
    {
        private List<Tarea> tareas;

        public GestorTareas()
        {
            tareas = new List<Tarea>();
        }

        public void Agregar(Tarea tarea)
        {
            tareas.Add(tarea);
        }

        public List<Tarea> ListarTodas()
        {
            return tareas;
        }

        public List<Tarea> ListarPorCategoria(string categoria)
        {
            return tareas
                .Where(t => t.Categoria != null &&
                            t.Categoria.Nombre.Equals(categoria, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Tarea> ListarPorPrioridad(Prioridad prioridad)
        {
            return tareas
                .Where(t => t.Prioridad == prioridad)
                .ToList();
        }

        public void Completar(int id)
        {
            Tarea tarea = tareas.FirstOrDefault(t => t.Id == id);

            if (tarea != null)
            {
                tarea.Completada = true;
            }
        }

        public List<Tarea> ObtenerVencidas()
        {
            return tareas
                .Where(t =>
                    t is TareaConVencimiento tv &&
                    tv.FechaVencimiento < DateTime.Now &&
                    !tv.Completada)
                .ToList();
        }

        public void Eliminar(int id)
        {
            Tarea tarea = tareas.FirstOrDefault(t => t.Id == id);

            if (tarea != null)
            {
                tareas.Remove(tarea);
            }
        }

        public void GuardarEnJSON(string ruta)
        {
            List<TareaDTO> datos = new List<TareaDTO>();

            foreach (Tarea t in tareas)
            {
                TareaDTO dto = new TareaDTO
                {
                    Tipo = t is TareaConVencimiento ? "TareaConVencimiento" : "Tarea",
                    Id = t.Id,
                    Titulo = t.Titulo,
                    Descripcion = t.Descripcion,
                    Prioridad = t.Prioridad.ToString(),
                    Completada = t.Completada,
                    FechaCreacion = t.FechaCreacion,
                    NombreCategoria = t.Categoria?.Nombre,
                    ColorCategoria = t.Categoria?.Color,
                    DescripcionCategoria = t.Categoria?.Descripcion
                };

                if (t is TareaConVencimiento tv)
                {
                    dto.FechaVencimiento = tv.FechaVencimiento;
                }

                datos.Add(dto);
            }

            JsonSerializerOptions opciones = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(datos, opciones);

            File.WriteAllText(ruta, json);
        }

        public void CargarDeJSON(string ruta)
        {
            if (!File.Exists(ruta))
                return;

            string json = File.ReadAllText(ruta);

            List<TareaDTO>? datos = JsonSerializer.Deserialize<List<TareaDTO>>(json);

            tareas.Clear();

            if (datos == null)
                return;

            foreach (TareaDTO dto in datos)
            {
                Categoria categoria = new Categoria
                {
                    Nombre = dto.NombreCategoria,
                    Color = dto.ColorCategoria,
                    Descripcion = dto.DescripcionCategoria
                };

                if (dto.Tipo == "TareaConVencimiento")
                {
                    TareaConVencimiento tarea = new TareaConVencimiento
                    {
                        Titulo = dto.Titulo,
                        Descripcion = dto.Descripcion,
                        Prioridad = Enum.Parse<Prioridad>(dto.Prioridad),
                        Categoria = categoria,
                        Completada = dto.Completada,
                        FechaVencimiento = dto.FechaVencimiento ?? DateTime.Now
                    };

                    tarea.RestaurarDatos(dto.Id, dto.FechaCreacion);

                    tareas.Add(tarea);

                }
                else
                {
                    Tarea tarea = new Tarea
                    {
                        Titulo = dto.Titulo,
                        Descripcion = dto.Descripcion,
                        Prioridad = Enum.Parse<Prioridad>(dto.Prioridad),
                        Categoria = categoria,
                        Completada = dto.Completada
                    };

                    tarea.RestaurarDatos(dto.Id, dto.FechaCreacion);

                    tareas.Add(tarea);
                }
            }
        }
    }
}