using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Servicos;
using minimal_api.Infraestrutura.Db;

namespace Test.Domain.Servicos
{
    [TestClass]
    public class AdministradorServicoTest
    {
        private DbContexto CriarContexto()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));
            var builder = new ConfigurationBuilder()
            .SetBasePath(path ?? Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

            var configuration = builder.Build();
            return new DbContexto(configuration);
        }
        private AdministradorServico CriarServico(DbContexto contexto)
        {
            var jwtService = new JwtService(new ConfigurationBuilder().Build());
            return new AdministradorServico(contexto, jwtService);
        }

        [TestMethod]
        public async Task Deve_Criar_Adm()
        {
            // Arrange
            var novoAdm = new Administrador
            {
                Id = 1,
                Email = "teste@teste.com",
                Senha = "teste123",
                Perfil = "ADM"
            };
            var contexto = CriarContexto();
            contexto.Database.ExecuteSqlRaw("TRUNCATE TABLE administradores");
            var administradorServico = CriarServico(contexto);

            // Act
            await administradorServico.Create(novoAdm);
            var quantidade = await administradorServico.Count();
            


            // Assert
            Assert.AreEqual(1, quantidade);
            Assert.AreEqual(1, administradorServico.FindById(novoAdm.Id).Id);
        }
    }
}