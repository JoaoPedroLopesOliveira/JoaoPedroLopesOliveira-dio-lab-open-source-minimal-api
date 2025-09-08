using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimal_api.Dominio.Entidades;

namespace minimal_api.Dominio.Interfaces
{
    public interface IVeiculo
    {
        List<Veiculo> FindAll(int pagina, string? nome = null, string? marca = null);
        Veiculo? FindById(int id);

        void Create(Veiculo veiculo);

        void Update(Veiculo veiculo);

        void DeleteById(int id);
    }
}