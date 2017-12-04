using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Common;
using SecretSanta.Factories;
using SecretSanta.Models;
using SecretSanta.Web.Controllers;
using SecretSanta.Web.Infrastructure;
using SecretSanta.Web.Models.Account;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace SecretSanta.Web.Tests.Controllers.AccountControllerTests
{
    [TestFixture]
    public class GenerateTokenTests
    {
        [TestCase("username", "password")]
        public async Task TestGenerateToken_ShouldCallAuthenticationProviderFindByUsernameAsyncCorrectly(string username,
          string password)
        {
            // Arrange
            var mockedFactory = new Mock<IUserFactory>();
            var mockedProvider = new Mock<IAuthenticationProvider>();
            var mockedDtoFactory = new Mock<IDtoFactory>();

            var controller = new AccountController(mockedProvider.Object, mockedFactory.Object, mockedDtoFactory.Object);

            var model = new LoginDto
            {
                Username = username,
                Password = password
            };

            // Act
            await controller.GenerateToken(model);

            // Assert
            mockedProvider.Verify(p => p.FindByUsernameAsync(username), Times.Once);
        }

        [TestCase("username", "password")]
        public async Task TestGenerateToken_NoUser_ShouldReturnBadRequest(string username, string password)
        {
            // Arrange
            var mockedFactory = new Mock<IUserFactory>();
            var mockedProvider = new Mock<IAuthenticationProvider>();
            var mockedDtoFactory = new Mock<IDtoFactory>();

            var controller = new AccountController(mockedProvider.Object, mockedFactory.Object, mockedDtoFactory.Object);

            var model = new LoginDto
            {
                Username = username,
                Password = password
            };

            // Act
            var result = await controller.GenerateToken(model);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [TestCase("username", "password")]
        public async Task TestGenerateToken_NoUser_ShouldReturnBadRequestWithCorrectMessage(string username, string password)
        {
            // Arrange
            var mockedFactory = new Mock<IUserFactory>();
            var mockedProvider = new Mock<IAuthenticationProvider>();
            var mockedDtoFactory = new Mock<IDtoFactory>();

            var controller = new AccountController(mockedProvider.Object, mockedFactory.Object, mockedDtoFactory.Object);

            var model = new LoginDto
            {
                Username = username,
                Password = password
            };

            // Act
            var result = await controller.GenerateToken(model) as BadRequestObjectResult;

            // Assert
            Assert.AreSame(Constants.InvalidCredentials, result.Value);
        }

        [TestCase("username", "password")]
        public async Task TestGenerateToken_ThereIsUser_ShouldCallAuthenticationProviderCheckPasswordSignInAsync(string username,
            string password)
        {
            // Arrange
            var mockedFactory = new Mock<IUserFactory>();
            var mockedDtoFactory = new Mock<IDtoFactory>();

            var user = new User();

            var mockedProvider = new Mock<IAuthenticationProvider>();
            mockedProvider.Setup(p => p.FindByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);
            mockedProvider.Setup(p => p.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(SignInResult.Success);

            var controller = new AccountController(mockedProvider.Object, mockedFactory.Object, mockedDtoFactory.Object);

            var model = new LoginDto
            {
                Username = username,
                Password = password
            };

            // Act
            await controller.GenerateToken(model);

            // Assert
            mockedProvider.Verify(p => p.CheckPasswordSignInAsync(user, password), Times.Once);
        }

        [TestCase("username", "password")]
        public async Task TestGenerateToken_ResultDoesNotSucceed_ShouldReturnBadRequest(string username,
            string password)
        {
            // Arrange
            var mockedFactory = new Mock<IUserFactory>();
            var mockedDtoFactory = new Mock<IDtoFactory>();

            var user = new User();

            var mockedProvider = new Mock<IAuthenticationProvider>();
            mockedProvider.Setup(p => p.FindByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);
            mockedProvider.Setup(p => p.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(SignInResult.Failed);

            var controller = new AccountController(mockedProvider.Object, mockedFactory.Object, mockedDtoFactory.Object);

            var model = new LoginDto
            {
                Username = username,
                Password = password
            };

            // Act
            var result = await controller.GenerateToken(model);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [TestCase("username", "password")]
        public async Task TestGenerateToken_ResultDoesNotSucceed_ShouldReturnBadRequestWithCorrectMessage(string username,
            string password)
        {
            // Arrange
            var mockedFactory = new Mock<IUserFactory>();
            var mockedDtoFactory = new Mock<IDtoFactory>();

            var user = new User();

            var mockedProvider = new Mock<IAuthenticationProvider>();
            mockedProvider.Setup(p => p.FindByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);
            mockedProvider.Setup(p => p.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(SignInResult.Failed);

            var controller = new AccountController(mockedProvider.Object, mockedFactory.Object, mockedDtoFactory.Object);

            var model = new LoginDto
            {
                Username = username,
                Password = password
            };

            // Act
            var result = await controller.GenerateToken(model) as BadRequestObjectResult;

            // Assert
            Assert.AreSame(Constants.InvalidCredentials, result.Value);
        }

        [TestCase("username", "password", "email")]
        public async Task TestGenerateToken_ResultIsSuccess_ShouldCallAuthenticationProviderGenerateToken(string username,
            string password,
            string email)
        {
            // Arrange
            var mockedFactory = new Mock<IUserFactory>();
            var mockedDtoFactory = new Mock<IDtoFactory>();

            var user = new User { Email = email };

            var mockedProvider = new Mock<IAuthenticationProvider>();
            mockedProvider.Setup(p => p.FindByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);
            mockedProvider.Setup(p => p.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(SignInResult.Success);

            var controller = new AccountController(mockedProvider.Object, mockedFactory.Object, mockedDtoFactory.Object);

            var model = new LoginDto
            {
                Username = username,
                Password = password
            };

            // Act
            await controller.GenerateToken(model);

            // Assert
            mockedProvider.Verify(p => p.GenerateToken(email));
        }

        [TestCase("username", "password", "token")]
        public async Task TestGenerateToken_ResultIsSuccess_ShouldCallDtoFactoryCreate(string username,
            string password,
            string token)
        {
            // Arrange
            var mockedFactory = new Mock<IUserFactory>();
            var mockedDtoFactory = new Mock<IDtoFactory>();

            var user = new User();

            var mockedProvider = new Mock<IAuthenticationProvider>();
            mockedProvider.Setup(p => p.FindByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);
            mockedProvider.Setup(p => p.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(SignInResult.Success);
            mockedProvider.Setup(p => p.GenerateToken(It.IsAny<string>())).Returns(token);

            var controller = new AccountController(mockedProvider.Object, mockedFactory.Object, mockedDtoFactory.Object);

            var model = new LoginDto
            {
                Username = username,
                Password = password
            };

            // Act
            await controller.GenerateToken(model);

            // Assert
            mockedDtoFactory.Verify(f => f.CreateTokenDto(token), Times.Once);
        }

        [TestCase("username", "password", "email")]
        public async Task TestGenerateToken_ResultIsSuccess_ShouldReturnOk(string username,
            string password,
            string email)
        {
            // Arrange
            var mockedFactory = new Mock<IUserFactory>();
            var mockedDtoFactory = new Mock<IDtoFactory>();

            var user = new User { Email = email };

            var mockedProvider = new Mock<IAuthenticationProvider>();
            mockedProvider.Setup(p => p.FindByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);
            mockedProvider.Setup(p => p.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(SignInResult.Success);

            var controller = new AccountController(mockedProvider.Object, mockedFactory.Object, mockedDtoFactory.Object);

            var model = new LoginDto
            {
                Username = username,
                Password = password
            };

            // Act
            var result = await controller.GenerateToken(model);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase("username", "password")]
        public async Task TestGenerateToken_ResultIsSuccess_ShouldReturnOkWithCorrectDto(string username,
            string password)
        {
            // Arrange
            var mockedFactory = new Mock<IUserFactory>();

            var dto = new TokenDto();
            var mockedDtoFactory = new Mock<IDtoFactory>();
            mockedDtoFactory.Setup(f => f.CreateTokenDto(It.IsAny<string>())).Returns(dto);

            var user = new User();

            var mockedProvider = new Mock<IAuthenticationProvider>();
            mockedProvider.Setup(p => p.FindByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);
            mockedProvider.Setup(p => p.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(SignInResult.Success);

            var controller = new AccountController(mockedProvider.Object, mockedFactory.Object, mockedDtoFactory.Object);

            var model = new LoginDto
            {
                Username = username,
                Password = password
            };

            // Act
            var result = await controller.GenerateToken(model) as OkObjectResult;

            // Assert
            Assert.AreSame(dto, result.Value);
        }
    }
}
