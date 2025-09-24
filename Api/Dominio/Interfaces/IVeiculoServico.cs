using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using minimal_api.Dominio.Entidades;

namespace minimal_api.Dominio.Interfaces
{
    public interface IVeiculoServico
    {
        Task <List<Veiculo>> FindAll(int pagina, string? nome = null, string? marca = null);
        Task <Veiculo?> FindById(int id);

        Task Create(Veiculo veiculo);

        Task Update(Veiculo veiculo);

        Task DeleteById(int id);
    }
}