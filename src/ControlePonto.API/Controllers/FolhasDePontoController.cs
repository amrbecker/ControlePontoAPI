using ControlePonto.Business.Interfaces;
using ControlePonto.Business.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ControlePonto.API.Controllers
{
    [ApiController]
	[Route("v1/folhas-de-ponto")]
	public class FolhasDePontoController : ControllerBase
	{
		private readonly IPontoService _pontoService;

        public FolhasDePontoController(IPontoService pontoService)
        {
            _pontoService = pontoService;
        }

        [HttpGet("{mes}")]
		[ProducesResponseType(typeof(FolhaPonto), 200)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<FolhaPonto> GeraRelatorioMensal(string mes)
		{
			var anoMes = mes.Split('-');
			var year = Convert.ToInt16(anoMes[0]);
			var month = Convert.ToInt16(anoMes[1]);

			var pontos = _pontoService.ObterPontos(null, month, year);

			if (pontos.Result == null)
				return NotFound("Relatório não encontrado");

			var horasDeTrabalhoMes = TimeSpan.FromHours(440);
			var horasTrabalhadas = CalcularHorasTrabalhadas(pontos.Result);
			var horasExcedentes = TimeSpan.Zero;
			var horasDevidas = TimeSpan.Zero;

			if (horasTrabalhadas > horasDeTrabalhoMes)
				horasExcedentes = horasTrabalhadas - horasDeTrabalhoMes;
			else
				horasDevidas = horasDeTrabalhoMes - horasTrabalhadas;

			var relatorio = new FolhaPonto
			{
				Mes = month,
				HorasTrabalhadas = horasTrabalhadas.ToString(@"hh\:mm\:ss"),
				HorasExcedentes = horasExcedentes.ToString(@"hh\:mm\:ss"),
				HorasDevidas = horasDevidas.ToString(@"hh\:mm\:ss")
			};

			relatorio.Registros = PreencherRelatorio(pontos.Result);

			// Retorne o relatório gerado
			return Ok(relatorio);
		}

		private static TimeSpan CalcularHorasTrabalhadas(List<Ponto> pontos)
		{
			var totalHoras = pontos.Sum(x => x.Hora) * 60 * 60;
			var totalMinutos = pontos.Sum(x => x.Minuto) * 60;
			var totalSegundos = pontos.Sum(x => x.Segundo) + totalHoras + totalMinutos;

			return TimeSpan.FromSeconds(totalSegundos);
		}

		private static List<Registro> PreencherRelatorio(List<Ponto> pontos)
		{
			var registros = new List<Registro>();

			for (var i = 0; i < 30; i++)
			{
				var pontosDoDia = pontos.Where(x => x.Dia == i);

				if (pontosDoDia.Any())
				{
					var registro = new Registro
					{
						Dia = new DateOnly(pontosDoDia.First().Ano, pontosDoDia.First().Mes, pontosDoDia.First().Dia)
					};

					foreach (var item in pontosDoDia)
					{
						registro.Horarios.Add(item.Hora + ":" + item.Minuto + ":" + item.Segundo);
					}

					registros.Add(registro);
				}
			}
			return registros;
		}

	}
}
