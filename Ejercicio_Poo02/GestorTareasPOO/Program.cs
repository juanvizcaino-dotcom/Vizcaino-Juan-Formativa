using GestorTareasPOO.Modelos;
using GestorTareasPOO.Servicios;

GestorTareas gestor = new GestorTareas();

string archivo = "tareas.json";

gestor.CargarDeJSON(archivo);

int opcion;

do
{
    Console.Clear();

    Console.WriteLine("===== GESTOR DE TAREAS =====");
    Console.WriteLine("1. Agregar tarea");
    Console.WriteLine("2. Listar todas");
    Console.WriteLine("3. Listar por categoría");
    Console.WriteLine("4. Listar por prioridad");
    Console.WriteLine("5. Marcar como completada");
    Console.WriteLine("6. Mostrar tareas vencidas");
    Console.WriteLine("7. Eliminar tarea");
    Console.WriteLine("8. Guardar tareas");
    Console.WriteLine("9. Salir");

    Console.Write("\nSeleccione una opción: ");

    if (!int.TryParse(Console.ReadLine(), out opcion))
    {
        Console.WriteLine("Opción inválida.");
        Console.ReadKey();
        continue;
    }

    switch (opcion)
    {
        case 1:

            Console.Write("Título: ");
            string titulo = Console.ReadLine();

            Console.Write("Descripción: ");
            string descripcion = Console.ReadLine();

            Console.WriteLine("\nPrioridad");
            Console.WriteLine("1. Baja");
            Console.WriteLine("2. Media");
            Console.WriteLine("3. Alta");
            Console.WriteLine("4. Critica");

            int p = int.Parse(Console.ReadLine());

            Prioridad prioridad = (Prioridad)(p - 1);

            Console.Write("Nombre categoría: ");
            string nombre = Console.ReadLine();

            Console.Write("Color: ");
            string color = Console.ReadLine();

            Console.Write("Descripción categoría: ");
            string descCat = Console.ReadLine();

            Categoria categoria = new Categoria
            {
                Nombre = nombre,
                Color = color,
                Descripcion = descCat
            };

            Console.Write("¿Tiene fecha de vencimiento? (S/N): ");
            string respuesta = Console.ReadLine().ToUpper();

            if (respuesta == "S")
            {
                Console.Write("Fecha (dd/MM/yyyy): ");

                DateTime fecha = DateTime.Parse(Console.ReadLine());

                TareaConVencimiento tarea = new TareaConVencimiento
                {
                    Titulo = titulo,
                    Descripcion = descripcion,
                    Prioridad = prioridad,
                    Categoria = categoria,
                    FechaVencimiento = fecha
                };

                gestor.Agregar(tarea);
            }
            else
            {
                Tarea tarea = new Tarea
                {
                    Titulo = titulo,
                    Descripcion = descripcion,
                    Prioridad = prioridad,
                    Categoria = categoria
                };

                gestor.Agregar(tarea);
            }

            Console.WriteLine("\nTarea agregada correctamente.");

            break;

        case 2:

            List<Tarea> lista = gestor.ListarTodas();

            if (lista.Count == 0)
            {
                Console.WriteLine("No hay tareas registradas.");
            }
            else
            {
                foreach (Tarea t in lista)
                {
                    t.MostrarInfo();
                    Console.WriteLine("--------------------------------");
                }
            }

            break;

        case 3:

            Console.Write("Categoría: ");

            string categoriaBuscar = Console.ReadLine();

            foreach (Tarea t in gestor.ListarPorCategoria(categoriaBuscar))
            {
                t.MostrarInfo();
                Console.WriteLine("--------------------------------");
            }

            break;

        case 4:

            Console.WriteLine("1 Baja");
            Console.WriteLine("2 Media");
            Console.WriteLine("3 Alta");
            Console.WriteLine("4 Critica");

            int pp = int.Parse(Console.ReadLine());

            Prioridad pr = (Prioridad)(pp - 1);

            foreach (Tarea t in gestor.ListarPorPrioridad(pr))
            {
                t.MostrarInfo();
                Console.WriteLine("--------------------------------");
            }

            break;

        case 5:

            Console.Write("ID: ");

            int id = int.Parse(Console.ReadLine());

            gestor.Completar(id);

            Console.WriteLine("Tarea completada.");

            break;

        case 6:

            foreach (Tarea t in gestor.ObtenerVencidas())
            {
                t.MostrarInfo();
                Console.WriteLine("--------------------------------");
            }

            break;

        case 7:

            Console.Write("ID: ");

            int eliminar = int.Parse(Console.ReadLine());

            gestor.Eliminar(eliminar);

            Console.WriteLine("Tarea eliminada.");

            break;

        case 8:

            gestor.GuardarEnJSON(archivo);

            Console.WriteLine("Datos guardados correctamente.");

            break;

        case 9:

            gestor.GuardarEnJSON(archivo);

            Console.WriteLine("Hasta luego.");

            break;

        default:

            Console.WriteLine("Opción incorrecta.");

            break;
    }

    if (opcion != 9)
    {
        Console.WriteLine("\nPresione una tecla para continuar...");
        Console.ReadKey();
    }

} while (opcion != 9);