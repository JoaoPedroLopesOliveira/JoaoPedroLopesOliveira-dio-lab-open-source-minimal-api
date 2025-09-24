using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using minimal_api.Dominio.Entidades;

namespace Test.Domain.Entidades
{
    [TestClass]
    public class VeiculoTest
    {
        [TestMethod]
        public void TestarGetSetPropriedades()
        {
            // Arange
            var veiculo = new Veiculo();

            // Act
            veiculo.Id = 1;
            veiculo.Ano = 2022;
            veiculo.Marca = "teste";
            veiculo.Nome = "teste.teste";

            //Assert
            Assert.AreEqual(1,veiculo.Id);
            Assert.AreEqual(2022, veiculo.Ano);
            Assert.AreEqual("teste",veiculo.Marca);
            Assert.AreEqual("teste.teste",veiculo.Nome);

        }
    }
}