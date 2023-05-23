namespace ControlePonto.Business.Models
{
    public class FolhaPonto
    {
        public int Mes { get; set; }
        public string HorasTrabalhadas { get; set; }
        public string HorasExcedentes { get; set; }
        public string HorasDevidas { get; set; }
        public List<Registro> Registros { get; set; } = new List<Registro>();
    }
}
