using ControlePonto.Business.Models;

namespace ControlePonto.Business.Interfaces
{
	public interface IPontoService
	{
		Task AdicionarPonto(Ponto ponto);
		Task<List<Ponto>> ObterPontos(int? dia, int mes, int ano);
	}
}
