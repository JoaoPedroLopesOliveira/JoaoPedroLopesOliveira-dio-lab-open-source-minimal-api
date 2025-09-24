using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using minimal_api.Dominio.DTOs;
using Test.Helpers;

namespace Test.Requests
{
    [TestClass]
public class AdministradorRequestTest
{
    [ClassInitialize]
    public static void ClassInit(TestContext context)
    {
        Setup.ClassInit(context);
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        Setup.ClassCleanup();
    }

    [TestMethod]
    public async Task Testar_Get_Set_Propriedades()
    {
        var loginDTO = new LoginDTO
        {
            Email = "adm@teste.com",
            Senha = "adm@123",
        };

        var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "application/json");

        var response = await Setup.client.PostAsync("administradores/login", content);

        Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
    }
}
}