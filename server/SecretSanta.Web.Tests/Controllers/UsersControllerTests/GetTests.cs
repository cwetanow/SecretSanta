using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SecretSanta.Models;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Controllers;
using SecretSanta.Web.Infrastructure;
using SecretSanta.Web.Models.Users;
using System.Collections.Generic;

namespace SecretSanta.Web.Tests.Controllers.UsersControllerTests
{
    [TestFixture]
    public class GetTests
    {
        [TestCase(0, 5, true, "name")]
        [TestCase(12, 7, false, "name")]
        [TestCase(3, 3, true, null)]
        [TestCase(0, 44, false, null)]
        public void TestGet_ShouldCallServiceGetUsersCorrectly(int offset, int limit, bool sortAscending, string searchPattern)
        {
            // Arrange
            var mockedDtoFactory = new Mock<IDtoFactory>();
            var mockedService = new Mock<IUserService>();

            var controller = new UsersController(mockedService.Object, mockedDtoFactory.Object);

            // Act
            controller.Get(offset, limit, sortAscending, searchPattern);

            // Assert
            mockedService.Verify(s => s.GetUsers(offset, limit, sortAscending, searchPattern));
        }

        [TestCase(0, 5, true, "name")]
        [TestCase(12, 7, false, "name")]
        public void TestGet_ShouldCallFactoryCreateCorrectly(int offset, int limit, bool sortAscending, string searchPattern)
        {
            // Arrange
            var users = new List<User>
            {
                new User(),
                new User()
            };

            var mockedDtoFactory = new Mock<IDtoFactory>();
            var mockedService = new Mock<IUserService>();
            mockedService.Setup(s => s.GetUsers(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()))
                .Returns(users);

            var controller = new UsersController(mockedService.Object, mockedDtoFactory.Object);

            // Act
            controller.Get(offset, limit, sortAscending, searchPattern);

            // Assert
            mockedDtoFactory.Verify(f => f.CreateUsersListDto(users), Times.Once);
        }

        [TestCase(0, 5, true, "name")]
        [TestCase(12, 7, false, "name")]
        public void TestGet_ShouldReturnOk(int offset, int limit, bool sortAscending, string searchPattern)
        {
            // Arrange
            var mockedDtoFactory = new Mock<IDtoFactory>();
            var mockedService = new Mock<IUserService>();

            var controller = new UsersController(mockedService.Object, mockedDtoFactory.Object);

            // Act
            var result = controller.Get(offset, limit, sortAscending, searchPattern);

            // Assert
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [TestCase(0, 5, true, "name")]
        [TestCase(12, 7, false, "name")]
        public void TestGet_ShouldSetCorrectBody(int offset, int limit, bool sortAscending, string searchPattern)
        {
            // Arrange

            var dto = new UsersListDto();
            var mockedDtoFactory = new Mock<IDtoFactory>();
            mockedDtoFactory.Setup(f => f.CreateUsersListDto(It.IsAny<IEnumerable<User>>()))
                .Returns(dto);

            var mockedService = new Mock<IUserService>();

            var controller = new UsersController(mockedService.Object, mockedDtoFactory.Object);

            // Act
            var result = controller.Get(offset, limit, sortAscending, searchPattern) as OkObjectResult;

            // Assert
            Assert.AreSame(dto, result.Value);
        }
    }
}
