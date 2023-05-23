namespace ControlePonto.Business.Models
{
    public class Registro
    {
		public DateOnly Dia { get; set; }
		public List<string> Horarios { get; set; } = new List<string>();
	}

}
