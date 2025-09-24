using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Entidades;

namespace minimal_api.Dominio.Interfaces
{
    public interface IAdministradorServico
    {
        Task<string?> Login(LoginDTO loginDTO);
        Task <Administrador>Create(Administrador administrador);
        Task Update(Administrador administrador);
        Task<List<Administrador>> FindAll(int pagina);
        Task <Administrador?> FindById(int id);
        Task DeleteById(int id);
    }
}