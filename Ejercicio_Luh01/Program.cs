using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static int totalValidas = 0;
    static int totalInvalidas = 0;

    static Dictionary<string, int> marcas = new Dictionary<string, int>()
    {
        {"Visa",0},
        {"Mastercard",0},
        {"American Express",0},
        {"Discover",0},
        {"Desconocida",0}
    };

    static Random random = new Random();

    static void Main(string[] args)
    {
        int opcion;

        do
        {
            Console.Clear();
            Console.WriteLine("=== VALIDADOR DE TARJETAS ===");
            Console.WriteLine("1. Validar una tarjeta");
            Console.WriteLine("2. Validar desde archivo");
            Console.WriteLine("3. Generar número válido");
            Console.WriteLine("4. Estadísticas");
            Console.WriteLine("5. Salir");
            Console.Write("Seleccione una opción: ");

            if (!int.TryParse(Console.ReadLine(), out opcion))
            {
                opcion = 0;
            }

            switch (opcion)
            {
                case 1:
                    ValidarManual();
                    break;

                case 2:
                    Console.Write("Ruta del archivo: ");
                    ValidarDesdeArchivo(Console.ReadLine());
                    break;

                case 3:
                    Console.WriteLine();
                    string numero = GenerarNumeroValido();
                    Console.WriteLine("Número generado: " + numero);
                    Console.WriteLine("Marca: " + IdentificarMarca(numero));
                    Console.ReadKey();
                    break;

                case 4:
                    MostrarEstadisticas();
                    break;

                case 5:
                    Console.WriteLine("Hasta luego.");
                    break;

                default:
                    Console.WriteLine("Opción inválida.");
                    Console.ReadKey();
                    break;
            }

        } while (opcion != 5);
    }

    static void ValidarManual()
    {
        Console.Write("\nIngrese el número de tarjeta: ");
        string numero = Console.ReadLine();

        bool valida = ValidarTarjeta(numero);
        string marca = IdentificarMarca(numero);

        Console.WriteLine("\nNúmero: " + numero);
        Console.WriteLine("Marca: " + marca);

        if (valida)
            Console.WriteLine("Estado: ✅ VÁLIDA");
        else
            Console.WriteLine("Estado: ❌ INVÁLIDA");

        ActualizarEstadisticas(valida, marca);

        Console.ReadKey();
    }

    static bool ValidarTarjeta(string numero)
    {
        if (string.IsNullOrWhiteSpace(numero))
            return false;

        foreach (char c in numero)
        {
            if (!char.IsDigit(c))
                return false;
        }

        int suma = 0;
        bool duplicar = false;

        for (int i = numero.Length - 1; i >= 0; i--)
        {
            int digito = numero[i] - '0';

            if (duplicar)
            {
                digito *= 2;

                if (digito > 9)
                    digito -= 9;
            }

            suma += digito;
            duplicar = !duplicar;
        }

        return suma % 10 == 0;
    }

    static string IdentificarMarca(string numero)
    {
        int longitud = numero.Length;

        if (numero.StartsWith("4") && (longitud == 13 || longitud == 16))
            return "Visa";

        if (longitud == 16)
        {
            int pref2 = int.Parse(numero.Substring(0, 2));

            if (pref2 >= 51 && pref2 <= 55)
                return "Mastercard";
        }

        if (longitud == 15 &&
            (numero.StartsWith("34") || numero.StartsWith("37")))
            return "American Express";

        if (longitud >= 16 && longitud <= 19)
        {
            if (numero.StartsWith("6011"))
                return "Discover";

            if (numero.StartsWith("65"))
                return "Discover";

            int pref3 = int.Parse(numero.Substring(0, 3));

            if (pref3 >= 644 && pref3 <= 649)
                return "Discover";

            int pref6 = int.Parse(numero.Substring(0, 6));

            if (pref6 >= 622126 && pref6 <= 622925)
                return "Discover";
        }

        return "Desconocida";
    }

    static void ValidarDesdeArchivo(string ruta)
    {
        try
        {
            string[] lineas = File.ReadAllLines(ruta);

            int validas = 0;
            int invalidas = 0;

            Console.WriteLine();

            foreach (string linea in lineas)
            {
                string numero = linea.Trim();

                bool estado = ValidarTarjeta(numero);
                string marca = IdentificarMarca(numero);

                Console.WriteLine(numero + " -> " + marca + " -> " +
                    (estado ? "VÁLIDA" : "INVÁLIDA"));

                if (estado)
                    validas++;
                else
                    invalidas++;

                ActualizarEstadisticas(estado, marca);
            }

            Console.WriteLine("\nResumen");
            Console.WriteLine("Válidas: " + validas);
            Console.WriteLine("Inválidas: " + invalidas);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

        Console.ReadKey();
    }

    static string GenerarNumeroValido()
    {
        string numero = "4";

        for (int i = 1; i < 15; i++)
        {
            numero += random.Next(10);
        }

        for (int ultimo = 0; ultimo <= 9; ultimo++)
        {
            string prueba = numero + ultimo;

            if (ValidarTarjeta(prueba))
                return prueba;
        }

        return "";
    }

    static void ActualizarEstadisticas(bool valida, string marca)
    {
        if (valida)
            totalValidas++;
        else
            totalInvalidas++;

        if (!marcas.ContainsKey(marca))
            marcas["Desconocida"]++;
        else
            marcas[marca]++;
    }

    static void MostrarEstadisticas()
    {
        Console.WriteLine("\n===== ESTADÍSTICAS =====");
        Console.WriteLine("Válidas: " + totalValidas);
        Console.WriteLine("Inválidas: " + totalInvalidas);

        Console.WriteLine("\nPor marca:");

        foreach (var item in marcas)
        {
            Console.WriteLine(item.Key + ": " + item.Value);
        }

        Console.ReadKey();
    }
}
