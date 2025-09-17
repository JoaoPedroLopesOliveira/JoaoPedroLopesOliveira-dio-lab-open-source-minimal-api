using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public async Task Create(Administrador administrador)
        {
            var hasher = new PasswordHasher<Administrador>();
            administrador.Senha = hasher.HashPassword(administrador, administrador.Senha);

            _contexto.Administradores.Add(administrador);
            await _contexto.SaveChangesAsync();
        }

        public async Task DeleteById(int id)
        {
            var administrador = await _contexto.Administradores.FindAsync(id);
            if (administrador != null)
            {
                _contexto.Administradores.Remove(administrador);
                await _contexto.SaveChangesAsync();
            }
        }

        public async Task<List<Administrador>> FindAll(int pagina = 1)
        {
            int tamanhoPagina = 10;
            return await _contexto.Administradores.Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToListAsync();
        }

        public async Task <Administrador?> FindById(int id)
        {
            return await _contexto.Administradores.Where(adm => adm.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Administrador?> Login(LoginDTO loginDTO)
        {
            var adm = await _contexto.Administradores.FirstOrDefaultAsync(a => a.Email == loginDTO.Email);
            if (adm is null) return null;

            var hasher = new PasswordHasher<Administrador>();
            var result = hasher.VerifyHashedPassword(adm, adm.Senha, loginDTO.Senha);
            return result == PasswordVerificationResult.Success ? adm : null;
        }

        public async Task Update(Administrador administrador)
        {
            var administradorExistente = await _contexto.Administradores.FindAsync(administrador.Id);
            if (administradorExistente == null)
            {
                throw new Exception("Administrador não encontrado");
            }
            _contexto.Administradores.Update(administrador);
            await _contexto.SaveChangesAsync();
        }
    }
}