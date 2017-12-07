using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Controllers;
using SecretSanta.Common;
using SecretSanta.Web.Infrastructure;
using SecretSanta.Models;
using SecretSanta.Web.Models.Users;

namespace SecretSanta.Web.Tests.Controllers.UsersControllerTests
{

    [TestFixture]
    public class GetByUsernameTests
    {
        [TestCase(null)]
        [TestCase("")]
        public void TestGetByUsername_UsernameEmpty_ShouldReturnBadRequest(string username)
        {
            // Arrange
            var mockedDtoFactory = new Mock<IDtoFactory>();
            var mockedService = new Mock<IUserService>();

            var controller = new UsersController(mockedService.Object, mockedDtoFactory.Object);

            // Act
            var result = controller.GetByUsername(username);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [TestCase(null)]
        [TestCase("")]
        public void TestGetByUsername_UsernameEmpty_ShouldSetCorrectMessageToResponse(string username)
        {
            // Arrange
            var mockedDtoFactory = new Mock<IDtoFactory>();
            var mockedService = new Mock<IUserService>();

            var controller = new UsersController(mockedService.Object, mockedDtoFactory.Object);

            // Act
            var result = controller.GetByUsername(username) as BadRequestObjectResult;

            // Assert
            Assert.AreSame(Constants.UsernameCannotBeNull, result.Value);
        }

        [TestCase("username")]
        [TestCase("test")]
        public void TestGetByUsername_UsernameNotEmpty_ShouldNotReturnBadRequest(string username)
        {
            // Arrange
            var mockedDtoFactory = new Mock<IDtoFactory>();
            var mockedService = new Mock<IUserService>();

            var controller = new UsersController(mockedService.Object, mockedDtoFactory.Object);

            // Act
            var result = controller.GetByUsername(username);

            // Assert
            Assert.IsNotInstanceOf<BadRequestObjectResult>(result);
        }

        [TestCase("username")]
        [TestCase("test")]
        public void TestGetByUsername_UsernameNotEmpty_ShouldCallServiceGetByUsername(string username)
        {
            // Arrange
            var mockedDtoFactory = new Mock<IDtoFactory>();
            var mockedService = new Mock<IUserService>();

            var controller = new UsersController(mockedService.Object, mockedDtoFactory.Object);

            // Act
            controller.GetByUsername(username);

            // Assert
            mockedService.Verify(s => s.GetByUsername(username), Times.Once);
        }

        [TestCase("username")]
        [TestCase("test")]
        public void TestGetByUsername_NoUserFound_ShouldReturnNotFound(string username)
        {
            // Arrange
            var mockedDtoFactory = new Mock<IDtoFactory>();
            var mockedService = new Mock<IUserService>();

            var controller = new UsersController(mockedService.Object, mockedDtoFactory.Object);

            // Act
            var result = controller.GetByUsername(username);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [TestCase("username")]
        [TestCase("test")]
        public void TestGetByUsername_NoUserFound_ShouldSetCorrectErrorMessage(string username)
        {
            // Arrange
            var mockedDtoFactory = new Mock<IDtoFactory>();
            var mockedService = new Mock<IUserService>();

            var controller = new UsersController(mockedService.Object, mockedDtoFactory.Object);

            // Act
            var result = controller.GetByUsername(username) as NotFoundObjectResult;

            // Assert
            Assert.AreSame(Constants.UserNotFound, result.Value);
        }

        [TestCase("username")]
        [TestCase("test")]
        public void TestGetByUsername_UserFound_ShouldNotReturnNotFound(string username)
        {
            // Arrange
            var mockedDtoFactory = new Mock<IDtoFactory>();
            var mockedService = new Mock<IUserService>();
            mockedService.Setup(s => s.GetByUsername(It.IsAny<string>())).Returns(new User());

            var controller = new UsersController(mockedService.Object, mockedDtoFactory.Object);

            // Act
            var result = controller.GetByUsername(username);

            // Assert
            Assert.IsNotInstanceOf<NotFoundObjectResult>(result);
        }

        [TestCase("username", "email", "display name")]
        public void TestGetByUsername_UserFound_ShouldCallFactoryCreate(string username, string email, string displayName)
        {
            // Arrange
            var mockedDtoFactory = new Mock<IDtoFactory>();

            var user = new User { UserName = username, Email = email, DisplayName = displayName };

            var mockedService = new Mock<IUserService>();
            mockedService.Setup(s => s.GetByUsername(It.IsAny<string>())).Returns(user);

            var controller = new UsersController(mockedService.Object, mockedDtoFactory.Object);

            // Act
            controller.GetByUsername(username);

            // Assert
            mockedDtoFactory.Verify(f => f.CreateUserDto(username, email, displayName), Times.Once);
        }

        [TestCase("username", "email", "display name")]
        public void TestGetByUsername_UserFound_ShouldReturnOk(string username, string email, string displayName)
        {
            // Arrange
            var mockedDtoFactory = new Mock<IDtoFactory>();

            var user = new User { UserName = username, Email = email, DisplayName = displayName };

            var mockedService = new Mock<IUserService>();
            mockedService.Setup(s => s.GetByUsername(It.IsAny<string>())).Returns(user);

            var controller = new UsersController(mockedService.Object, mockedDtoFactory.Object);

            // Act
            var result = controller.GetByUsername(username);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase("username", "email", "display name")]
        public void TestGetByUsername_UserFound_ShouldSetCorrectBody(string username, string email, string displayName)
        {
            // Arrange
            var dto = new UserDto();
            var mockedDtoFactory = new Mock<IDtoFactory>();
            mockedDtoFactory.Setup(f => f.CreateUserDto(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(dto);

            var user = new User { UserName = username, Email = email, DisplayName = displayName };

            var mockedService = new Mock<IUserService>();
            mockedService.Setup(s => s.GetByUsername(It.IsAny<string>())).Returns(user);

            var controller = new UsersController(mockedService.Object, mockedDtoFactory.Object);

            // Act
            var result = controller.GetByUsername(username) as OkObjectResult;

            // Assert
            Assert.AreSame(dto, result.Value);
        }
    }
}
