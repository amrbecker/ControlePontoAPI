using ControlePonto.Business.Interfaces;
using ControlePonto.Business.Models;
using ControlePonto.Data.Context;
using Microsoft.AspNetCore.Mvc;

namespace ControlePonto.API.Controllers
{
    [ApiController]
	[Route("v1/batidas")]
	public class BatidasController : ControllerBase
	{
		private readonly IPontoService _pontoService;

        public BatidasController(IPontoService pontoService)
        {
			_pontoService = pontoService;
        }

        [HttpPost]
		[ProducesResponseType(typeof(Batida), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public ActionResult<Registro> InsereBatida([FromBody] Batida batida)
		{
			if(!ModelState.IsValid)
				return BadRequest("mensagem: Data e hora em formato inválido");

			var ponto = new Ponto
			{
				Dia = batida.DataHora.Day,
				Mes = batida.DataHora.Month,
				Ano = batida.DataHora.Year,
				Hora = batida.DataHora.Hour,
				Minuto = batida.DataHora.Minute,
				Segundo = batida.DataHora.Second
			};

			var registro = new Registro
			{
				Dia = new DateOnly(ponto.Ano, ponto.Mes, ponto.Dia)
			};

			var pontos = _pontoService.ObterPontos(ponto.Dia, ponto.Mes, ponto.Ano);

			//Verifica Regras de Negócio

			if (pontos == null)
				_pontoService.AdicionarPonto(ponto);

			else if (pontos.Result.Any(x => x.Hora == ponto.Hora && x.Minuto == ponto.Minuto && x.Segundo == ponto.Segundo))
				return Conflict("mensagem: Horário já registrado");

			else if (pontos.Result.Count() == 4)
				return new ContentResult
				{
					StatusCode = 403,
					Content = "mensagem: Apenas 4 horários podem ser registrados por dia",
					ContentType = "text/plain"
				};

			else
			{
				_pontoService.AdicionarPonto(ponto);

				foreach (var item in pontos.Result) 
				{
					registro.Horarios.Add(item.Hora + ":" + item.Minuto + ":" + item.Segundo);
				}
			}

			registro.Horarios.Add(ponto.Hora + ":" + ponto.Minuto + ":" + ponto.Segundo);

			// Retorne o registro criado
			return Created("", registro);
		}
	}
	
}
