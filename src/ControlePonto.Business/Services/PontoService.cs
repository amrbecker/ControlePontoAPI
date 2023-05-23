using ControlePonto.Business.Interfaces;
using ControlePonto.Business.Models;

namespace ControlePonto.Business.Services
{
	public class PontoService : IPontoService
	{
		private readonly IPontoRepository _repository;

        public PontoService(IPontoRepository pontoRepository)
        {
            _repository = pontoRepository;
        }

        public async Task AdicionarPonto(Ponto ponto)
		{
			await _repository.AdicionarPonto(ponto);
		}

		public async Task<List<Ponto>> ObterPontos(int? dia, int mes, int ano)
		{
			return await _repository.ObterPontos(dia, mes, ano);
		}
	}
}
