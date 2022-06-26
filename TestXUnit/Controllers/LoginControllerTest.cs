using Chapter1000ton.Controllers;
using Chapter1000ton.Interfaces;
using Chapter1000ton.Models;
using Chapter1000ton.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestXUnit.Controllers
{
    public class LoginControllerTeste
    {
        [Fact]
        public void LoginController_Retornar_Usuario_Invalid()
        {
            // Arrange
            var fakeRepository = new Mock<IUsuarioRepository>();
            fakeRepository.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns((Usuario)null);

            LoginViewModel dadosLogin = new LoginViewModel();

            dadosLogin.Email = "emailqualquer@teste.com";
            dadosLogin.Senha = "123";

            var controller = new LoginController(fakeRepository.Object);

            // Act
            var resultado = controller.Login(dadosLogin);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(resultado);
        }

        [Fact]
        public void LoginController_Retornar_Token()
        {
            //Arrange
            Usuario usuarioRetorno = new Usuario();
            usuarioRetorno.Email = "emailqualquer@teste.com";
            usuarioRetorno.Senha = "123";
            usuarioRetorno.Tipo = "0";

            var fakeRepository = new Mock<IUsuarioRepository>();
            fakeRepository.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(usuarioRetorno);

            string issuerValidacao = "chapter.webapi";

            LoginViewModel dadosLogin = new LoginViewModel();

            dadosLogin.Email = "emailqualquer@teste.com";
            dadosLogin.Senha = "123";

            var controller = new LoginController(fakeRepository.Object);

            //Act
            OkObjectResult resultado = (OkObjectResult)controller.Login(dadosLogin);

            string token = resultado.Value.ToString().Split(' ')[3];

            var jwtHandler = new JwtSecurityTokenHandler();

            var tokenJwt = jwtHandler.ReadJwtToken(token);

            // Assert
            Assert.Equal(issuerValidacao, tokenJwt.Issuer);
        }
    }
}
