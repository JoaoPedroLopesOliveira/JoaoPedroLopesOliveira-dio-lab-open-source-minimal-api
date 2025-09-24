using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;
using minimal_api.Infraestrutura.Db;

namespace minimal_api.Dominio.Servicos
{
    public class VeiculosServico(DbContexto contexto) : IVeiculoServico
    {
        private readonly DbContexto _contexto = contexto;

        public async Task Create(Veiculo veiculo)
        {
           await _contexto.Veiculos.AddAsync(veiculo);
           await _contexto.SaveChangesAsync();
        }

        public async Task DeleteById(int id)
        {
            var veiculo = await _contexto.Veiculos.FindAsync(id);
            if (veiculo != null)
            {
                _contexto.Veiculos.Remove(veiculo);
                await _contexto.SaveChangesAsync();
            }
        }

        public async Task <List<Veiculo>> FindAll(int pagina = 1, string? nome = null, string? marca = null)
        {
            int tamanhoPagina = 10;
            var query = _contexto.Veiculos.AsQueryable();
            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(v => EF.Functions.Like(v.Nome, $"%{nome}%"));
            }
            if (!string.IsNullOrWhiteSpace(marca))
            {
                query = query.Where(v => EF.Functions.Like(v.Marca, $"%{marca}%"));
            }
            return await query.OrderBy(v => v.Id).Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToListAsync();
        }

        public async Task <Veiculo?> FindById(int id)
        {
            return await _contexto.Veiculos.Where(v => v.Id == id).FirstOrDefaultAsync();
        }

        public async Task Update(Veiculo veiculo)
        {
            var veiculoExistente = _contexto.Veiculos.Find(veiculo.Id);
            if (veiculoExistente == null)
            {
                throw new Exception("Veiculo não encontrado!");
            }
            _contexto.Veiculos.Update(veiculo);
            await _contexto.SaveChangesAsync();
        }


    }
}