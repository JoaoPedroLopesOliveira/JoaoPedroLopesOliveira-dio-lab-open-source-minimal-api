using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.Entidades;

namespace minimal_api.Infraestrutura.Db
{
    public class DbContexto : DbContext
    {
        private readonly IConfiguration _configuracaoAppSetings;
        public DbContexto(IConfiguration configuracaoAppSetings)
        {
            _configuracaoAppSetings = configuracaoAppSetings;
        }
        public DbSet<Administrador> Administradores { get; set; } = default!;
        public DbSet<Veiculo> Veiculos { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder){}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var stringConexao = _configuracaoAppSetings.GetConnectionString("mysql")?.ToString();
            if (!string.IsNullOrEmpty(stringConexao))
            {
                optionsBuilder.UseMySql(stringConexao, ServerVersion.AutoDetect(stringConexao));
            }
        }
    }
}