using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Controllers;
using SecretSanta.Web.Infrastructure;
using System.Threading.Tasks;
using SecretSanta.Common;
using SecretSanta.Models;
using System.Collections.Generic;
using SecretSanta.Web.Models.Users;

namespace SecretSanta.Web.Tests.Controllers.GroupControllerTests
{
    [TestFixture]
    public class GetGroupUsersTests
    {
        [TestCase("group name", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("name", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestGetGroupUsers_GroupNameEmpty_ShouldReturnBadRequest(string groupName, string userId)
        {
            // Arrange
            var mockedService = new Mock<IGroupService>();
            var mockedFactory = new Mock<IDtoFactory>();
            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();

			var mockedMembershipService = new Mock<IMembershipService>();

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object, mockedMembershipService.Object);

			// Act
			var result = await controller.GetGroupUsers(null);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [TestCase("group name", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("name", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestGetGroupUsers_GroupNameEmpty_ShouldSetCorrectMessage(string groupName, string userId)
        {
            // Arrange
            var mockedService = new Mock<IGroupService>();
            var mockedFactory = new Mock<IDtoFactory>();
            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();

			var mockedMembershipService = new Mock<IMembershipService>();

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object, mockedMembershipService.Object);

			// Act
			var result = await controller.GetGroupUsers(null) as BadRequestObjectResult;

            // Assert
            Assert.AreSame(Constants.GroupNameCannotBeNull, result.Value);
        }

        [TestCase("group name", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("name", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestGetGroupUsers_GroupNameNotEmpty_ShouldCallAuthenticationProviderGetCurrentUserAsync(string groupName, string userId)
        {
            // Arrange
            var group = new Group { OwnerId = userId };

            var mockedService = new Mock<IGroupService>();
            mockedService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

            var mockedFactory = new Mock<IDtoFactory>();

            var user = new User { Id = userId };

            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var mockedMembershipService = new Mock<IMembershipService>();

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object, mockedMembershipService.Object);

			// Act
			await controller.GetGroupUsers(groupName);

            // Assert
            mockedAuthenticationProvider.Verify(p => p.GetCurrentUserAsync(), Times.Once);
        }

        [TestCase("group name", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("name", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestGetGroupUsers_GroupNameNotEmpty_ShouldCallGroupServiceGetByName(string groupName, string userId)
        {
            // Arrange
            var group = new Group { OwnerId = userId };

            var mockedService = new Mock<IGroupService>();
            mockedService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

            var mockedFactory = new Mock<IDtoFactory>();

            var user = new User { Id = userId };

            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var mockedMembershipService = new Mock<IMembershipService>();

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object, mockedMembershipService.Object);

			// Act
			await controller.GetGroupUsers(groupName);

            // Assert
            mockedService.Verify(s => s.GetByName(groupName), Times.Once);
        }

        [TestCase("group name", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("name", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestGetGroupUsers_ShouldCallServiceGetGroupUsers(string groupName, string userId)
        {
            // Arrange
            var group = new Group { OwnerId = userId };

            var mockedService = new Mock<IGroupService>();
            mockedService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

            var mockedFactory = new Mock<IDtoFactory>();

            var user = new User { Id = userId };

            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var mockedMembershipService = new Mock<IMembershipService>();

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object, mockedMembershipService.Object);

			// Act
			await controller.GetGroupUsers(groupName);

            // Assert
            mockedService.Verify(s => s.GetGroupUsers(groupName), Times.Once);
        }

        [TestCase("group name", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("name", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestGetGroupUsers_ShouldCallFactoryCreate(string groupName, string userId)
        {
            // Arrange
            var group = new Group { OwnerId = userId };

            var user = new User { Id = userId };
            var users = new List<User> { user };

            var mockedService = new Mock<IGroupService>();
            mockedService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);
            mockedService.Setup(s => s.GetGroupUsers(It.IsAny<string>())).Returns(users);

            var mockedFactory = new Mock<IDtoFactory>();

            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var mockedMembershipService = new Mock<IMembershipService>();

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object, mockedMembershipService.Object);

			// Act
			await controller.GetGroupUsers(groupName);

            // Assert
            mockedFactory.Verify(f => f.CreateUsersListDto(users), Times.Once);
        }

        [TestCase("group name", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("name", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestGetGroupUsers_ShouldReturnOk(string groupName, string userId)
        {
            // Arrange
            var group = new Group { OwnerId = userId };

            var user = new User { Id = userId };
            var users = new List<User>();

            var mockedService = new Mock<IGroupService>();
            mockedService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);
            mockedService.Setup(s => s.GetGroupUsers(It.IsAny<string>())).Returns(users);

            var mockedFactory = new Mock<IDtoFactory>();

            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var mockedMembershipService = new Mock<IMembershipService>();

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object, mockedMembershipService.Object);

			// Act
			var result = await controller.GetGroupUsers(groupName);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase("group name", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("name", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestGetGroupUsers_ShouldSetCorrectBody(string groupName, string userId)
        {
            // Arrange
            var group = new Group { OwnerId = userId };

            var user = new User { Id = userId };
            var users = new List<User>();

            var mockedService = new Mock<IGroupService>();
            mockedService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);
            mockedService.Setup(s => s.GetGroupUsers(It.IsAny<string>())).Returns(users);

            var dto = new UsersListDto();

            var mockedFactory = new Mock<IDtoFactory>();
            mockedFactory.Setup(f => f.CreateUsersListDto(It.IsAny<IEnumerable<User>>())).Returns(dto);

            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var mockedMembershipService = new Mock<IMembershipService>();

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object, mockedMembershipService.Object);

			// Act
			var result = await controller.GetGroupUsers(groupName) as OkObjectResult;

            // Assert
            Assert.AreSame(dto, result.Value);
        }
    }
}
