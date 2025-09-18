using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using minimal_api.Dominio.Entidades;

namespace minimal_api.Dominio.Servicos
{
    public class JwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateToken(Administrador administrador)
        {
            var claims = new[] {
                new Claim (ClaimTypes.Name, administrador.Email),
                new Claim (ClaimTypes.Role, administrador.Perfil)
            };
            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Key"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credenciais
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}