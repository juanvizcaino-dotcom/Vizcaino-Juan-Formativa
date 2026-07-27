namespace GestorTareasPOO.Modelos
{
    public class TareaConVencimiento : Tarea
    {
        public DateTime FechaVencimiento { get; set; }

        public int DiasRestantes
        {
            get
            {
                return (FechaVencimiento - DateTime.Now).Days;
            }
        }

        public TareaConVencimiento() : base()
        {

        }

        public override void MostrarInfo()
        {
            base.MostrarInfo();
            Console.WriteLine($"Fecha de vencimiento: {FechaVencimiento}");
            Console.WriteLine($"Días restantes: {DiasRestantes}");
        }
    }
}