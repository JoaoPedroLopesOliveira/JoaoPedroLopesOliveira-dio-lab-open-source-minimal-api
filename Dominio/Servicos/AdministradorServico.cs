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
        private readonly JwtService _jwtservice;
        public AdministradorServico(DbContexto contexto, JwtService jwtService)
        {
            _contexto = contexto;
            _jwtservice = jwtService;
        }

        public async Task<Administrador> Create(Administrador administrador)
        {
            var existe = await _contexto.Administradores.AnyAsync(a => a.Email == administrador.Email);
            if (existe)
            {
                throw new Exception("Ja existe um administrador com este email");
            }

            var hasher = new PasswordHasher<Administrador>();
            administrador.Senha = hasher.HashPassword(administrador, administrador.Senha);

            _contexto.Administradores.Add(administrador);
            await _contexto.SaveChangesAsync();
            return administrador;
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
            return await _contexto.Administradores.Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina).Select(a => new Administrador
            {
                Id = a.Id,
                Email = a.Email,
                Senha = a.Senha,
                Perfil = a.Perfil
            }).ToListAsync();
        }

        public async Task <Administrador?> FindById(int id)
        {
            return await _contexto.Administradores.Where(adm => adm.Id == id).FirstOrDefaultAsync();
        }

        public async Task<String?> Login(LoginDTO loginDTO)
        {
            var adm = await _contexto.Administradores.FirstOrDefaultAsync(a => a.Email == loginDTO.Email);
            if (adm is null) return null;

            var hasher = new PasswordHasher<Administrador>();
            var result = hasher.VerifyHashedPassword(adm, adm.Senha, loginDTO.Senha);
            if (result == PasswordVerificationResult.Failed) return null;

            return _jwtservice.GenerateToken(adm);
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