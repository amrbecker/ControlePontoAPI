namespace ControlePonto.Business.Models
{
	public class Ponto
	{
        public Ponto()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
		public int Dia { get; set; }
		public int Mes { get; set; }
		public int Ano { get; set; }
		public int Hora { get; set; }
		public int Minuto { get; set; }
		public int Segundo { get; set; }
	}
}
