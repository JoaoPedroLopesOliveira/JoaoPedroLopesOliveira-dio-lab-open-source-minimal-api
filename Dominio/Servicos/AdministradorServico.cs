using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;
using minimal_api.Infraestrutura.Db;

namespace minimal_api.Dominio.Servicos
{
    public class AdministradorServico : IAdministradorServico
    {
        private readonly DbContexto _contexto;
        public AdministradorServico(DbContexto contexto)
        {
            _contexto = contexto;
        }

        public void Create(Administrador administrador)
        {
            _contexto.Administradores.Add(administrador);
            _contexto.SaveChanges();
        }

        public List<Administrador> FindAll(int pagina = 1)
        {
            int tamanhoPagina = 10;
            return _contexto.Administradores.Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToList();
        }

        public Administrador? FindById(int id)
        {
            return _contexto.Administradores.Where(adm => adm.Id == id).FirstOrDefault();
        }

        public Administrador? Login(LoginDTO loginDTO)
        {
            var adm = _contexto.Administradores.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
            return adm;  
        }

        public void Update(Administrador administrador)
        {
            var administradorExistente = _contexto.Administradores.Find(administrador.Id);
            if (administradorExistente == null)
            {
                throw new Exception("Administrador não encontrado");
            }
            _contexto.Administradores.Update(administrador);
            _contexto.SaveChanges();
        }
    }
}