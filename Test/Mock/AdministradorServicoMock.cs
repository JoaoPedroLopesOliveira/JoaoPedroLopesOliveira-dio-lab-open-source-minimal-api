using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;

namespace Test.Mock
{
    public class AdministradorServicoMock : IAdministradorServico
    {
        private static List<Administrador> administradores = new List<Administrador>()
        {
            new Administrador {
                Id = 1,
                Email = "adm@teste.com",
                Senha = "adm@123",
                Perfil = "ADM"
            },
            new Administrador{
                Id = 2,
                Email = "adm2@teste.com",
                Senha = "adm@223",
                Perfil = "ADM"
            }
        };

        public Task<Administrador> Create(Administrador administrador)
        {
            administrador.Id = administradores.Count + 1;
            administradores.Add(administrador);
            return Task.FromResult(administrador);
        }

        public Task DeleteById(int id)
        {
            var adm = administradores.FirstOrDefault(a => a.Id == id);
            if (adm != null)
            {
                administradores.Remove(adm);
            }
            return Task.CompletedTask;
        }

        public Task<List<Administrador>> FindAll(int pagina)
        {
            return Task.FromResult(administradores.ToList());
        }

        public Task<Administrador?> FindById(int id)
        {
            var adm = administradores.FirstOrDefault(a => a.Id == id);
            return Task.FromResult(adm);
        }

        public Task<string?> Login(LoginDTO loginDTO)
        {
            var adm = administradores.FirstOrDefault(a => a.Email == loginDTO.Email);
            if (adm == null || adm.Senha != loginDTO.Senha)
            {
                return Task.FromResult<string?>(null);
            }
            
            return Task.FromResult<string?>("fake-jwt-token");
        }

        public Task Update(Administrador administrador)
        {
            var adm = administradores.FirstOrDefault(a => a.Id == administrador.Id);
            if (adm != null)
            {
                adm.Email = administrador.Email;
                adm.Senha = administrador.Senha;
                adm.Perfil = administrador.Perfil;
            }
            return Task.CompletedTask;
        }
    }
}