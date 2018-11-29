using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Models;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Controllers;
using SecretSanta.Web.Models.Membership;
using System.Threading.Tasks;

namespace SecretSanta.Web.Tests.Controllers.MembershipControllerTests
{
	[TestFixture]
	public class AnswerInviteTests
	{
		[TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "groupName")]
		[TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "testGroup")]
		public async Task TestAnswerInvite_ShouldCallAuthenticationProviderGetCurrentUserAsync(int groupId,
			string userId, string groupName)
		{
			// Arrange
			var mockedMembershipService = new Mock<IMembershipService>();
			var mockedInviteService = new Mock<IInviteService>();
			var mockedGroupService = new Mock<IGroupService>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();

			var controller = new MembershipController(mockedMembershipService.Object, mockedInviteService.Object, mockedGroupService.Object,
				mockedAuthenticationProvider.Object);

			var dto = new AnswerInviteDto { GroupName = groupName, Accepted = true };

			// Act
			await controller.AnswerInvite(dto);

			// Assert
			mockedAuthenticationProvider.Verify(p => p.GetCurrentUserAsync(), Times.Once);
		}

		[TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "groupName")]
		[TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "testGroup")]
		public async Task TestAnswerInvite_ShouldCallGroupServiceGetByName(int groupId,
			string userId, string groupName)
		{
			// Arrange
			var mockedMembershipService = new Mock<IMembershipService>();
			var mockedInviteService = new Mock<IInviteService>();
			var mockedGroupService = new Mock<IGroupService>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();

			var controller = new MembershipController(mockedMembershipService.Object, mockedInviteService.Object, mockedGroupService.Object,
				mockedAuthenticationProvider.Object);

			var dto = new AnswerInviteDto { GroupName = groupName, Accepted = true };

			// Act
			await controller.AnswerInvite(dto);

			// Assert
			mockedGroupService.Verify(s => s.GetByName(groupName), Times.Once);
		}

		[TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "groupName")]
		[TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "testGroup")]
		public async Task TestAnswerInvite_NoGroup_ShouldReturnNotFound(int groupId,
			string userId, string groupName)
		{
			// Arrange
			var mockedMembershipService = new Mock<IMembershipService>();
			var mockedInviteService = new Mock<IInviteService>();
			var mockedGroupService = new Mock<IGroupService>();
			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();

			var controller = new MembershipController(mockedMembershipService.Object, mockedInviteService.Object, mockedGroupService.Object,
				mockedAuthenticationProvider.Object);

			var dto = new AnswerInviteDto { GroupName = groupName, Accepted = true };

			// Act
			var result = await controller.AnswerInvite(dto);

			// Assert
			Assert.IsInstanceOf<NotFoundResult>(result);
		}

		[TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "groupName")]
		[TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "testGroup")]
		public async Task TestAnswerInvite_NoUser_ShouldReturnNotFound(int groupId,
			string userId, string groupName)
		{
			// Arrange
			var mockedMembershipService = new Mock<IMembershipService>();
			var mockedInviteService = new Mock<IInviteService>();

			var group = new Group();

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();

			var controller = new MembershipController(mockedMembershipService.Object, mockedInviteService.Object, mockedGroupService.Object,
				mockedAuthenticationProvider.Object);

			var dto = new AnswerInviteDto { GroupName = groupName, Accepted = true };

			// Act
			var result = await controller.AnswerInvite(dto);

			// Assert
			Assert.IsInstanceOf<NotFoundResult>(result);
		}

		[TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "groupName")]
		[TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "testGroup")]
		public async Task TestAnswerInvite_ShouldCallInviteServiceIsUserInvited(int groupId,
			string userId, string groupName)
		{
			// Arrange
			var mockedMembershipService = new Mock<IMembershipService>();
			var mockedInviteService = new Mock<IInviteService>();

			var group = new Group { Id = groupId };

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var controller = new MembershipController(mockedMembershipService.Object, mockedInviteService.Object, mockedGroupService.Object,
				mockedAuthenticationProvider.Object);

			var dto = new AnswerInviteDto { GroupName = groupName, Accepted = true };

			// Act
			await controller.AnswerInvite(dto);

			// Assert
			mockedInviteService.Verify(s => s.IsUserInvited(groupId, userId), Times.Once);
		}

		[TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "groupName")]
		[TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "testGroup")]
		public async Task TestAnswerInvite_IsNotInvited_ShouldReturnForbidden(int groupId,
			string userId, string groupName)
		{
			// Arrange
			var mockedMembershipService = new Mock<IMembershipService>();
			var mockedInviteService = new Mock<IInviteService>();

			var group = new Group { Id = groupId };

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var controller = new MembershipController(mockedMembershipService.Object, mockedInviteService.Object, mockedGroupService.Object,
				mockedAuthenticationProvider.Object);

			var dto = new AnswerInviteDto { GroupName = groupName, Accepted = true };

			// Act
			var result = await controller.AnswerInvite(dto);

			// Assert
			Assert.IsInstanceOf<ForbidResult>(result);
		}

		[TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "groupName")]
		[TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "testGroup")]
		public async Task TestAnswerInvite_AcceptsInvite_ShouldCallMembershipServiceJoinGroup(int groupId,
			string userId, string groupName)
		{
			// Arrange
			var mockedMembershipService = new Mock<IMembershipService>();

			var mockedInviteService = new Mock<IInviteService>();
			mockedInviteService.Setup(s => s.IsUserInvited(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

			var group = new Group { Id = groupId };

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var controller = new MembershipController(mockedMembershipService.Object, mockedInviteService.Object, mockedGroupService.Object,
				mockedAuthenticationProvider.Object);

			var dto = new AnswerInviteDto { GroupName = groupName, Accepted = true };

			// Act
			await controller.AnswerInvite(dto);

			// Assert
			mockedMembershipService.Verify(s => s.JoinGroup(groupId, userId), Times.Once);
		}

		[TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "groupName")]
		[TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "testGroup")]
		public async Task TestAnswerInvite_AcceptsInvite_ShouldNotCallInviteServiceCancelInvite(int groupId,
			string userId, string groupName)
		{
			// Arrange
			var mockedMembershipService = new Mock<IMembershipService>();

			var mockedInviteService = new Mock<IInviteService>();
			mockedInviteService.Setup(s => s.IsUserInvited(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

			var group = new Group { Id = groupId };

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.FindByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);

			var controller = new MembershipController(mockedMembershipService.Object, mockedInviteService.Object, mockedGroupService.Object,
				mockedAuthenticationProvider.Object);

			var dto = new AnswerInviteDto { GroupName = groupName, Accepted = true };

			// Act
			await controller.AnswerInvite(dto);

			// Assert
			mockedInviteService.Verify(s => s.RemoveInvite(groupId, userId), Times.Never);
		}

		[TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "groupName")]
		[TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "testGroup")]
		public async Task TestAnswerInvite_DoesNotAcceptInvite_ShouldNotCallMembershipServiceJoinGroup(int groupId,
			string userId, string groupName)
		{
			// Arrange
			var mockedMembershipService = new Mock<IMembershipService>();

			var mockedInviteService = new Mock<IInviteService>();
			mockedInviteService.Setup(s => s.IsUserInvited(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

			var group = new Group { Id = groupId };

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.FindByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);

			var controller = new MembershipController(mockedMembershipService.Object, mockedInviteService.Object, mockedGroupService.Object,
				mockedAuthenticationProvider.Object);

			var dto = new AnswerInviteDto { GroupName = groupName, Accepted = false };

			// Act
			await controller.AnswerInvite(dto);

			// Assert
			mockedMembershipService.Verify(s => s.JoinGroup(groupId, userId), Times.Never);
		}

		[TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "groupName")]
		[TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "testGroup")]
		public async Task TestAnswerInvite_DoesNotAcceptInvite_ShouldCallInviteServiceCancelInvite(int groupId,
			string userId, string groupName)
		{
			// Arrange
			var mockedMembershipService = new Mock<IMembershipService>();

			var mockedInviteService = new Mock<IInviteService>();
			mockedInviteService.Setup(s => s.IsUserInvited(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

			var group = new Group { Id = groupId };

			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

			var user = new User { Id = userId };

			var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
			mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

			var controller = new MembershipController(mockedMembershipService.Object, mockedInviteService.Object, mockedGroupService.Object,
				mockedAuthenticationProvider.Object);

			var dto = new AnswerInviteDto { GroupName = groupName, Accepted = false };

			// Act
			await controller.AnswerInvite(dto);

			// Assert
			mockedInviteService.Verify(s => s.RemoveInvite(groupId, userId), Times.Once);
		}
	}
}
