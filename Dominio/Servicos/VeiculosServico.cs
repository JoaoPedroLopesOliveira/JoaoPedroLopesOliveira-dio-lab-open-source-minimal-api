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
    public class VeiculosServico(DbContexto contexto) : IVeiculo
    {
        private readonly DbContexto _contexto = contexto;

        public void Create(Veiculo veiculo)
        {
            _contexto.Veiculos.Add(veiculo);
            _contexto.SaveChanges();
        }

        public void DeleteById(int id)
        {
            var veiculo = _contexto.Veiculos.Find(id);
            if (veiculo != null)
            {
                _contexto.Veiculos.Remove(veiculo);
                _contexto.SaveChanges();
            }
        }

        public List<Veiculo> FindAll(int pagina, string? nome = null, string? marca = null)
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

            return query.OrderBy(v => v.Id).Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToList();
        }

        public Veiculo? FindById(int id)
        {
            return _contexto.Veiculos.Where(v => v.Id == id).FirstOrDefault();
        }

        public void Update(Veiculo veiculo)
        {
            var veiculoExistente = _contexto.Veiculos.Find(veiculo.Id);
            if (veiculoExistente == null)
            {
                throw new Exception("Veiculo não encontrado!");
            }
            _contexto.Veiculos.Update(veiculo);
            _contexto.SaveChanges();
        }


    }
}