using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Models;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Controllers;
using SecretSanta.Web.Infrastructure;
using SecretSanta.Web.Models.Group;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta.Web.Tests.Controllers.GroupControllerTests
{
    [TestFixture]
    public class GetUserGroupsTests
    {
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestGetUserGroups_ShouldCallAuthenticationProviderGetCurrentUserAsync(string userId)
        {
            // Arrange
            var mockedService = new Mock<IGroupService>();
            var mockedFactory = new Mock<IDtoFactory>();

            var user = new User { Id = userId };

            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

            var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

            // Act
            await controller.GetUserGroups();

            // Assert
            mockedAuthenticationProvider.Verify(p => p.GetCurrentUserAsync(), Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestGetUserGroups_ShouldCallServiceGetUserGroups(string userId)
        {
            // Arrange
            var mockedService = new Mock<IGroupService>();
            var mockedFactory = new Mock<IDtoFactory>();

            var user = new User { Id = userId };

            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

            var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

            // Act
            await controller.GetUserGroups();

            // Assert
            mockedService.Verify(s => s.GetUserGroups(userId), Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestGetUserGroups_ShouldCallFactoryCreate(string userId)
        {
            // Arrange
            var groups = new List<Group>();

            var mockedService = new Mock<IGroupService>();
            mockedService.Setup(s => s.GetUserGroups(It.IsAny<string>())).Returns(groups);

            var mockedFactory = new Mock<IDtoFactory>();

            var user = new User { Id = userId };

            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

            var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

            // Act
            await controller.GetUserGroups();

            // Assert
            mockedFactory.Verify(f => f.CreateGroupListDto(groups), Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestGetUserGroups_ShouldReturnOk(string userId)
        {
            // Arrange
            var mockedService = new Mock<IGroupService>();

            var dto = new GroupListDto();

            var mockedFactory = new Mock<IDtoFactory>();
            mockedFactory.Setup(f => f.CreateGroupListDto(It.IsAny<IEnumerable<Group>>())).Returns(dto);

            var user = new User { Id = userId };

            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

            var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

            // Act
            var result = await controller.GetUserGroups();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestGetUserGroups_ShouldSetCorrectBody(string userId)
        {
            // Arrange
            var mockedService = new Mock<IGroupService>();

            var dto = new GroupListDto();

            var mockedFactory = new Mock<IDtoFactory>();
            mockedFactory.Setup(f => f.CreateGroupListDto(It.IsAny<IEnumerable<Group>>())).Returns(dto);

            var user = new User { Id = userId };

            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

            var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

            // Act
            var result = await controller.GetUserGroups() as OkObjectResult;

            // Assert
            Assert.AreSame(dto, result.Value);
        }
    }
}
