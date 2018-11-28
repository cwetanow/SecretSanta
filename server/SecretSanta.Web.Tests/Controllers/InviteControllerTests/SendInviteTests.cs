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
using SecretSanta.Web.Models.Invite;
using SecretSanta.Web.Models.Users;

namespace SecretSanta.Web.Tests.Controllers.InviteControllerTests
{
	[TestFixture]
	public class SendInviteTests
	{
		[Test]
		public async Task TestSendInvite_PassEmptyGroupName_ShouldReturnBadRequest()
		{
			// Arrange
			var mockedService = new Mock<IInviteService>();
			var mockedFactory = new Mock<IDtoFactory>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			var mockedGroupService = new Mock<IGroupService>();

			var controller = new InviteController(mockedAuthenticationProvider.Object,
				mockedService.Object, mockedFactory.Object, mockedGroupService.Object);

			var dto = new UserDto { Username = string.Empty };

			// Act
			var result = await controller.SendInvite(null, dto);

			// Assert
			Assert.IsInstanceOf<BadRequestObjectResult>(result);
		}

		[Test]
		public async Task TestSendInvite_PassEmptyGroupName_ShouldSetCorrectErrorMessage()
		{
			// Arrange
			var mockedService = new Mock<IInviteService>();
			var mockedFactory = new Mock<IDtoFactory>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			var mockedGroupService = new Mock<IGroupService>();

			var controller = new InviteController(mockedAuthenticationProvider.Object,
				mockedService.Object, mockedFactory.Object, mockedGroupService.Object);

			var dto = new UserDto { Username = string.Empty };

			// Act
			var result = await controller.SendInvite(null, dto) as BadRequestObjectResult;

			// Assert
			Assert.AreSame(Constants.GroupNameCannotBeNull, result.Value);
		}

		[TestCase("group name")]
		public async Task TestSendInvite_PassEmptyUsername_ShouldReturnBadRequest(string groupName)
		{
			// Arrange
			var mockedService = new Mock<IInviteService>();
			var mockedFactory = new Mock<IDtoFactory>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			var mockedGroupService = new Mock<IGroupService>();

			var controller = new InviteController(mockedAuthenticationProvider.Object,
				mockedService.Object, mockedFactory.Object, mockedGroupService.Object);

			var dto = new UserDto { Username = string.Empty };

			// Act
			var result = await controller.SendInvite(null, dto);

			// Assert
			Assert.IsInstanceOf<BadRequestObjectResult>(result);
		}

		[TestCase("group name")]
		public async Task TestSendInvite_PassEmptyUsername_ShouldSetCorrectErrorMessage(string groupName)
		{
			// Arrange
			var mockedService = new Mock<IInviteService>();
			var mockedFactory = new Mock<IDtoFactory>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			var mockedGroupService = new Mock<IGroupService>();

			var controller = new InviteController(mockedAuthenticationProvider.Object,
				mockedService.Object, mockedFactory.Object, mockedGroupService.Object);

			var dto = new UserDto { Username = string.Empty };

			// Act
			var result = await controller.SendInvite(groupName, dto) as BadRequestObjectResult;

			// Assert
			Assert.AreSame(Constants.UsernameCannotBeNull, result.Value);
		}

		[TestCase("group name", "username")]
		public async Task TestSendInvite_ShouldCallGroupServiceGetByName(string groupName, string username)
		{
			// Arrange
			var mockedService = new Mock<IInviteService>();
			var mockedFactory = new Mock<IDtoFactory>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			var mockedGroupService = new Mock<IGroupService>();

			var controller = new InviteController(mockedAuthenticationProvider.Object,
				mockedService.Object, mockedFactory.Object, mockedGroupService.Object);

			var dto = new UserDto { Username = username };

			// Act
			var result = await controller.SendInvite(groupName, dto);

			// Assert
			mockedGroupService.Verify(s => s.GetByName(groupName), Times.Once);
		}

		[TestCase("group name", "username")]
		public async Task TestSendInvite_NoGroupFound_ShouldReturnNotFound(string groupName, string username)
		{
			// Arrange
			var mockedService = new Mock<IInviteService>();
			var mockedFactory = new Mock<IDtoFactory>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			var mockedGroupService = new Mock<IGroupService>();

			var controller = new InviteController(mockedAuthenticationProvider.Object,
				mockedService.Object, mockedFactory.Object, mockedGroupService.Object);

			var dto = new UserDto { Username = username };

			// Act
			var result = await controller.SendInvite(groupName, dto);

			// Assert
			Assert.IsInstanceOf<NotFoundResult>(result);
		}

		[TestCase("group name", "username", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		public async Task TestSendInvite_ThereIsGroup_ShouldCallAuthenticationProviderGetCurrentUserAsync(string groupName, string username, string userId)
		{
			// Arrange
			var mockedService = new Mock<IInviteService>();
			var mockedFactory = new Mock<IDtoFactory>();

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var group = new Group();

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var controller = new InviteController(mockedAuthenticationProvider.Object,
				mockedService.Object, mockedFactory.Object, mockedGroupService.Object);

			var dto = new UserDto { Username = username };

			// Act
			var result = await controller.SendInvite(groupName, dto);

			// Assert
			mockedAuthenticationProvider.Verify(p => p.GetCurrentUserAsync(), Times.Once);
		}

		[TestCase("group name", "username", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		public async Task TestSendInvite_GroupOwnerDoesNotMatchUser_ShouldReturnForbidden(string groupName, string username, string userId)
		{
			// Arrange
			var mockedService = new Mock<IInviteService>();
			var mockedFactory = new Mock<IDtoFactory>();

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var group = new Group();

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var controller = new InviteController(mockedAuthenticationProvider.Object,
				mockedService.Object, mockedFactory.Object, mockedGroupService.Object);

			var dto = new UserDto { Username = username };

			// Act
			var result = await controller.SendInvite(groupName, dto);

			// Assert
			Assert.IsInstanceOf<ForbidResult>(result);
		}

		[TestCase("group name", "username", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		public async Task TestSendInvite_GroupOwnerMatchesUser_ShouldCallAuthenticationProviderFindByUsernameAsync(string groupName,
			string username, string userId)
		{
			// Arrange
			var mockedService = new Mock<IInviteService>();
			var mockedFactory = new Mock<IDtoFactory>();

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var group = new Group { OwnerId = userId };

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var controller = new InviteController(mockedAuthenticationProvider.Object,
				mockedService.Object, mockedFactory.Object, mockedGroupService.Object);

			var dto = new UserDto { Username = username };

			// Act
			var result = await controller.SendInvite(groupName, dto);

			// Assert
			mockedAuthenticationProvider.Verify(p => p.FindByUsernameAsync(username), Times.Once);
		}

		[TestCase("group name", "username", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		public async Task TestSendInvite_NoUserFound_ShouldReturnNotFound(string groupName,
			string username, string userId)
		{
			// Arrange
			var mockedService = new Mock<IInviteService>();
			var mockedFactory = new Mock<IDtoFactory>();

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var group = new Group { OwnerId = userId };

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var controller = new InviteController(mockedAuthenticationProvider.Object,
				mockedService.Object, mockedFactory.Object, mockedGroupService.Object);

			var dto = new UserDto { Username = username };

			// Act
			var result = await controller.SendInvite(groupName, dto);

			// Assert
			Assert.IsInstanceOf<NotFoundResult>(result);
		}

		[TestCase("group name", "username", "d547a40d-c45f-4c43-99de-0bfe9199ff95", 3)]
		public async Task TestSendInvite_ThereIsUser_ShouldCallServiceCreateInviteAsync(string groupName,
			string username, string userId, int groupId)
		{
			// Arrange
			var mockedService = new Mock<IInviteService>();
			var mockedFactory = new Mock<IDtoFactory>();

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);
			mockedAuthenticationProvider.Setup(p => p.FindByUsernameAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			var group = new Group { OwnerId = userId, Id = groupId };

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var controller = new InviteController(mockedAuthenticationProvider.Object,
				mockedService.Object, mockedFactory.Object, mockedGroupService.Object);

			var dto = new UserDto { Username = username };

			// Act
			var result = await controller.SendInvite(groupName, dto);

			// Assert
			mockedService.Verify(s => s.CreateInviteAsync(groupId, userId), Times.Once);
		}

		[TestCase("group name", "username", "d547a40d-c45f-4c43-99de-0bfe9199ff95", 3)]
		public async Task TestSendInvite_ThereIsUser_ShouldCallFactoryCreate(string groupName,
			string username, string userId, int groupId)
		{
			// Arrange
			var invite = new Invite();

			var mockedService = new Mock<IInviteService>();
			mockedService.Setup(s => s.CreateInviteAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(invite);

			var mockedFactory = new Mock<IDtoFactory>();

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);
			mockedAuthenticationProvider.Setup(p => p.FindByUsernameAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			var group = new Group { OwnerId = userId, Id = groupId };

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var controller = new InviteController(mockedAuthenticationProvider.Object,
				mockedService.Object, mockedFactory.Object, mockedGroupService.Object);

			var dto = new UserDto { Username = username };

			// Act
			var result = await controller.SendInvite(groupName, dto);

			// Assert
			mockedFactory.Verify(f => f.CreateInviteDto(invite), Times.Once);
		}

		[TestCase("group name", "username", "d547a40d-c45f-4c43-99de-0bfe9199ff95", 3)]
		public async Task TestSendInvite_ShouldReturnOk(string groupName,
			string username, string userId, int groupId)
		{
			// Arrange
			var invite = new Invite();

			var mockedService = new Mock<IInviteService>();
			mockedService.Setup(s => s.CreateInviteAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(invite);

			var mockedFactory = new Mock<IDtoFactory>();

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);
			mockedAuthenticationProvider.Setup(p => p.FindByUsernameAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			var group = new Group { OwnerId = userId, Id = groupId };

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var controller = new InviteController(mockedAuthenticationProvider.Object,
				mockedService.Object, mockedFactory.Object, mockedGroupService.Object);

			var dto = new UserDto { Username = username };

			// Act
			var result = await controller.SendInvite(groupName, dto);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
		}

		[TestCase("group name", "username", "d547a40d-c45f-4c43-99de-0bfe9199ff95", 3)]
		public async Task TestSendInvite_ShouldSetCorrectBody(string groupName,
			string username, string userId, int groupId)
		{
			// Arrange
			var invite = new Invite();

			var mockedService = new Mock<IInviteService>();
			mockedService.Setup(s => s.CreateInviteAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(invite);

			var dto = new InviteDto();

			var mockedFactory = new Mock<IDtoFactory>();
			mockedFactory.Setup(s => s.CreateInviteDto(It.IsAny<Invite>())).Returns(dto);

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);
			mockedAuthenticationProvider.Setup(p => p.FindByUsernameAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			var group = new Group { OwnerId = userId, Id = groupId };

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var controller = new InviteController(mockedAuthenticationProvider.Object,
				mockedService.Object, mockedFactory.Object, mockedGroupService.Object);

			var userDto = new UserDto { Username = username };

			// Act
			var result = await controller.SendInvite(groupName, userDto) as OkObjectResult;

			// Assert
			Assert.AreSame(dto, result.Value);
		}
	}
}
