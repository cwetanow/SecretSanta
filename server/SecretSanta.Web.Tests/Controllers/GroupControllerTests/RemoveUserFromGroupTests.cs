using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Models;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Controllers;
using SecretSanta.Web.Infrastructure;
using SecretSanta.Web.Models.Users;
using System.Threading.Tasks;

namespace SecretSanta.Web.Tests.Controllers.GroupControllerTests
{
	[TestFixture]
	public class RemoveUserFromGroupTests
	{
		[TestCase("group name", "username", "d547a40d-c45f-4c43-99de-0bfe9199ff95", 3)]
		public async Task TestRemoveUserFromGroup_ShouldCallAuthenticationProviderGetCurrentUserAsync(string groupName,
			string username, string userId, int groupId)
		{
			// Arrange
			var mockedService = new Mock<IGroupService>();
			var mockedFactory = new Mock<IDtoFactory>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

			var dto = new UserDto { Username = username };

			// Act
			await controller.RemoveUserFromGroup(groupName, dto);

			// Assert
			mockedAuthenticationProvider.Verify(p => p.GetCurrentUserAsync(), Times.Once);
		}

		[TestCase("group name", "username", "d547a40d-c45f-4c43-99de-0bfe9199ff95", 3)]
		public async Task TestRemoveUserFromGroup_ShouldCallGroupServiceGetByName(string groupName,
			string username, string userId, int groupId)
		{
			// Arrange
			var mockedService = new Mock<IGroupService>();
			var mockedFactory = new Mock<IDtoFactory>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

			var dto = new UserDto { Username = username };

			// Act
			await controller.RemoveUserFromGroup(groupName, dto);

			// Assert
			mockedService.Verify(s => s.GetByName(groupName), Times.Once);
		}

		[TestCase("group name", "username", "d547a40d-c45f-4c43-99de-0bfe9199ff95", 3)]
		public async Task TestRemoveUserFromGroup_ThereIsNoGroup_ShouldReturnNotFound(string groupName,
			string username, string userId, int groupId)
		{
			// Arrange
			var mockedService = new Mock<IGroupService>();
			var mockedFactory = new Mock<IDtoFactory>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

			var dto = new UserDto { Username = username };

			// Act
			var result = await controller.RemoveUserFromGroup(groupName, dto);

			// Assert
			Assert.IsInstanceOf<NotFoundResult>(result);
		}

		[TestCase("group name", "username", "d547a40d-c45f-4c43-99de-0bfe9199ff95", 3)]
		public async Task TestRemoveUserFromGroup_UserIsNotOwner_ShouldReturnForbidden(string groupName,
			string username, string userId, int groupId)
		{
			// Arrange
			var group = new Group();

			var mockedService = new Mock<IGroupService>();
			mockedService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var mockedFactory = new Mock<IDtoFactory>();

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

			var dto = new UserDto { Username = username };

			// Act
			var result = await controller.RemoveUserFromGroup(groupName, dto);

			// Assert
			Assert.IsInstanceOf<ForbidResult>(result);
		}

		[TestCase("group name", "username", "d547a40d-c45f-4c43-99de-0bfe9199ff95", 3)]
		public async Task TestRemoveUserFromGroup_UserIsOwner_ShouldCallAuthenticationProviderFindByUsernameAsync(string groupName,
			string username, string userId, int groupId)
		{
			// Arrange
			var group = new Group { OwnerId = userId };

			var mockedService = new Mock<IGroupService>();
			mockedService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var mockedFactory = new Mock<IDtoFactory>();

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

			var dto = new UserDto { Username = username };

			// Act
			await controller.RemoveUserFromGroup(groupName, dto);

			// Assert
			mockedAuthenticationProvider.Verify(p => p.FindByUsernameAsync(username), Times.Once);
		}

		[TestCase("group name", "username", "d547a40d-c45f-4c43-99de-0bfe9199ff95", 3)]
		public async Task TestRemoveUserFromGroup_NoUserFound_ShouldReturnNotFound(string groupName,
			string username, string userId, int groupId)
		{
			// Arrange
			var group = new Group { OwnerId = userId };

			var mockedService = new Mock<IGroupService>();
			mockedService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var mockedFactory = new Mock<IDtoFactory>();

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

			var dto = new UserDto { Username = username };

			// Act
			var result = await controller.RemoveUserFromGroup(groupName, dto);

			// Assert
			Assert.IsInstanceOf<NotFoundResult>(result);
		}

		[TestCase("group name", "username", "d547a40d-c45f-4c43-99de-0bfe9199ff95", 3)]
		public async Task TestRemoveUserFromGroup_UserFound_ShouldCallGroupServiceRemoveUserFromGroup(string groupName,
			string username, string userId, int groupId)
		{
			// Arrange
			var group = new Group { OwnerId = userId, Id = groupId };

			var mockedService = new Mock<IGroupService>();
			mockedService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var mockedFactory = new Mock<IDtoFactory>();

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);
			mockedAuthenticationProvider.Setup(p => p.FindByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

			var dto = new UserDto { Username = username };

			// Act
			await controller.RemoveUserFromGroup(groupName, dto);

			// Assert
			mockedService.Verify(s => s.RemoveUserFromGroup(groupId, userId), Times.Once);
		}

		[TestCase("group name", "username", "d547a40d-c45f-4c43-99de-0bfe9199ff95", 3)]
		public async Task TestRemoveUserFromGroup_UserFound_ShouldReturnNoContent(string groupName,
			string username, string userId, int groupId)
		{
			// Arrange
			var group = new Group { OwnerId = userId, Id = groupId };

			var mockedService = new Mock<IGroupService>();
			mockedService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var mockedFactory = new Mock<IDtoFactory>();

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);
			mockedAuthenticationProvider.Setup(p => p.FindByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object);

			var dto = new UserDto { Username = username };

			// Act
			var result = await controller.RemoveUserFromGroup(groupName, dto);

			// Assert
			Assert.IsInstanceOf<NoContentResult>(result);
		}
	}
}
