using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using minimal_api.Dominio.Entidades;

namespace Test.Domain.Entidades
{
    [TestClass]
    public class AdministradorTest
    {
        [TestMethod]
        public void TestarGetSetPropriedades()
        {
            // Arange
            var adm = new Administrador();

            // Act
            adm.Id = 1;
            adm.Email = "teste@teste.com";
            adm.Senha = "teste123";
            adm.Perfil = "ADM";

            // Assert

            Assert.AreEqual(1, adm.Id);
            Assert.AreEqual("teste@teste.com", adm.Email);
            Assert.AreEqual("teste123", adm.Senha);
            Assert.AreEqual("ADM", adm.Perfil);
        }
    }
}