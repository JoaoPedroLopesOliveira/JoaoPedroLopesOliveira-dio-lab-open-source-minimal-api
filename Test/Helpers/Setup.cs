using System.Net.Http; 
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration; // precisa desse using
using minimal_api;
using minimal_api.Dominio.Interfaces;
using Test.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Test.Helpers
{
    [TestClass] 
    public class Setup
    {
        public const string PORT = "5001";
        public static TestContext TestContext { get; set; } = default!;
        public static WebApplicationFactory<Startup> http = default!;
        public static HttpClient client = default!;

        [ClassInitialize] 
        public static void ClassInit(TestContext context)
        {
            TestContext = context;
            http = new WebApplicationFactory<Startup>();

            http = http.WithWebHostBuilder(builder =>
            {
                builder.UseSetting("https_port", PORT).UseEnvironment("Testing");

                // 🔑 Configuração em memória para o Jwt:Key
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["Jwt:Key"] = "chave-fake-para-testes"
                    });
                });

                builder.ConfigureServices(services =>
                {
                    services.AddScoped<IAdministradorServico, AdministradorServicoMock>();
                });
            });

            client = http.CreateClient();
        }
    
        [ClassCleanup] 
        public static void ClassCleanup()
        {
            http.Dispose();
        }
    }
}
