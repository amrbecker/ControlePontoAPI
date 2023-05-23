using ControlePonto.Business.Interfaces;
using ControlePonto.Business.Models;
using ControlePonto.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;

namespace ControlePonto.Data.Repository
{
	public class PontoRepository : IPontoRepository
	{
		protected readonly ControlePontoDbContext Db;
		protected readonly DbSet<Ponto> DbSet;

        public PontoRepository(ControlePontoDbContext db)
        {
			Db = db;
			DbSet = db.Set<Ponto>();
        }

        public async Task AdicionarPonto(Ponto ponto)
		{
			DbSet.Add(ponto);
			await Db.SaveChangesAsync();
			
		}

		public async Task<List<Ponto>> ObterPontos(int? dia, int mes, int ano)
		{
			if (dia == null)
				return await Db.Pontos.Where(x=> x.Mes == mes && x.Ano == ano)
					.OrderBy(x => x.Ano)
					.OrderBy(x => x.Mes)
					.OrderBy(x => x.Dia)
					.OrderBy(x => x.Hora)
					.OrderBy(x => x.Minuto)
					.OrderBy(x => x.Segundo)
					.ToListAsync();
			else
				return await Db.Pontos.Where(x => x.Dia == dia && x.Mes == mes && x.Ano == ano)
					.OrderBy(x => x.Ano)
					.OrderBy(x => x.Mes)
					.OrderBy(x => x.Dia)
					.OrderBy(x => x.Hora)
					.OrderBy(x => x.Minuto)
					.OrderBy(x => x.Segundo)
					.ToListAsync();
		}
	}
}
