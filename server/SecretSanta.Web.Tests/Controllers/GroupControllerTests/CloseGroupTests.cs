using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Models;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Controllers;
using SecretSanta.Web.Infrastructure;

namespace SecretSanta.Web.Tests.Controllers.GroupControllerTests
{
	[TestFixture]
	public class CloseGroupTests
	{
		[TestCase("group name", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase("name", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public async Task TestCloseGroup_GroupNameEmpty_ShouldReturnBadRequest(string groupName, string userId)
		{
			// Arrange
			var mockedService = new Mock<IGroupService>();
			var mockedFactory = new Mock<IDtoFactory>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();

			var mockedMembershipService = new Mock<IMembershipService>();

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object, mockedMembershipService.Object);

			// Act
			var result = await controller.CloseGroup(null);

			// Assert
			Assert.IsInstanceOf<BadRequestObjectResult>(result);
		}

		[TestCase("group name", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase("name", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public async Task TestCloseGroup_GroupNameNotEmpty_ShouldCallAuthenticationProviderGetCurrentUserAsync(string groupName, string userId)
		{
			// Arrange
			var mockedService = new Mock<IGroupService>();
			var mockedFactory = new Mock<IDtoFactory>();

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var mockedMembershipService = new Mock<IMembershipService>();

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object, mockedMembershipService.Object);

			// Act
			await controller.CloseGroup(groupName);

			// Assert
			mockedAuthenticationProvider.Verify(p => p.GetCurrentUserAsync(), Times.Once);
		}

		[TestCase("group name", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase("name", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public async Task TestCloseGroup_GroupNameNotEmpty_ShouldCallGroupServiceIsUserOwner(string groupName, string userId)
		{
			// Arrange
			var mockedService = new Mock<IGroupService>();
			var mockedFactory = new Mock<IDtoFactory>();

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var mockedMembershipService = new Mock<IMembershipService>();

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object, mockedMembershipService.Object);

			// Act
			await controller.CloseGroup(groupName);

			// Assert
			mockedService.Verify(s => s.IsUserOwner(groupName, userId), Times.Once);
		}

		[TestCase("group name", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase("name", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public async Task TestCloseGroup_UserIsNotOwner_ShouldReturnForbidden(string groupName, string userId)
		{
			// Arrange
			var mockedService = new Mock<IGroupService>();
			mockedService.Setup(s => s.IsUserOwner(It.IsAny<string>(), It.IsAny<string>()))
				.Returns(false);

			var mockedFactory = new Mock<IDtoFactory>();

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var mockedMembershipService = new Mock<IMembershipService>();

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object, mockedMembershipService.Object);

			// Act
			var result = await controller.CloseGroup(groupName);

			// Assert
			Assert.IsInstanceOf<ForbidResult>(result);
		}

		[TestCase("group name", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase("name", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public async Task TestCloseGroup_UserIsOwner_ShouldCallGroupServiceCloseGroup(string groupName, string userId)
		{
			// Arrange
			var mockedService = new Mock<IGroupService>();
			mockedService.Setup(s => s.IsUserOwner(It.IsAny<string>(), It.IsAny<string>()))
				.Returns(true);

			var mockedFactory = new Mock<IDtoFactory>();

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var mockedMembershipService = new Mock<IMembershipService>();

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object, mockedMembershipService.Object);

			// Act
			await controller.CloseGroup(groupName);

			// Assert
			mockedService.Verify(s => s.CloseGroup(groupName), Times.Once);
		}

		[TestCase("group name", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase("name", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public async Task TestCloseGroup_UserIsOwner_ShouldReturnNoContent(string groupName, string userId)
		{
			// Arrange
			var mockedService = new Mock<IGroupService>();
			mockedService.Setup(s => s.IsUserOwner(It.IsAny<string>(), It.IsAny<string>()))
				.Returns(true);

			var mockedFactory = new Mock<IDtoFactory>();

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var mockedMembershipService = new Mock<IMembershipService>();

			var controller = new GroupController(mockedService.Object, mockedFactory.Object, mockedAuthenticationProvider.Object, mockedMembershipService.Object);

			// Act
			var result = await controller.CloseGroup(groupName);

			// Assert
			Assert.IsInstanceOf<NoContentResult>(result);
		}
	}
}
