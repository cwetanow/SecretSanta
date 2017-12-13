using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Models;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Controllers;
using System.Threading.Tasks;

namespace SecretSanta.Web.Tests.Controllers.MembershipControllerTests
{
    [TestFixture]
    public class AnswerInviteTests
    {
        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "groupName", "username")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "testGroup", "testUsername")]
        public async Task TestAnswerInvite_ShouldCallAuthenticationProviderFindByUsernameAsync(int groupId,
            string userId, string groupName, string username)
        {
            // Arrange
            var mockedMembershipService = new Mock<IMembershipService>();
            var mockedInviteService = new Mock<IInviteService>();
            var mockedGroupService = new Mock<IGroupService>();
            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();

            var controller = new MembershipController(mockedMembershipService.Object, mockedInviteService.Object, mockedGroupService.Object,
                mockedAuthenticationProvider.Object);

            // Act
            await controller.AnswerInvite(groupName, username, true);

            // Assert
            mockedAuthenticationProvider.Verify(p => p.FindByUsernameAsync(username), Times.Once);
        }

        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "groupName", "username")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "testGroup", "testUsername")]
        public async Task TestAnswerInvite_ShouldCallGroupServiceGetByName(int groupId,
            string userId, string groupName, string username)
        {
            // Arrange
            var mockedMembershipService = new Mock<IMembershipService>();
            var mockedInviteService = new Mock<IInviteService>();
            var mockedGroupService = new Mock<IGroupService>();
            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();

            var controller = new MembershipController(mockedMembershipService.Object, mockedInviteService.Object, mockedGroupService.Object,
                mockedAuthenticationProvider.Object);

            // Act
            await controller.AnswerInvite(groupName, username, true);

            // Assert
            mockedGroupService.Verify(s => s.GetByName(groupName), Times.Once);
        }

        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "groupName", "username")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "testGroup", "testUsername")]
        public async Task TestAnswerInvite_NoGroup_ShouldReturnNotFound(int groupId,
            string userId, string groupName, string username)
        {
            // Arrange
            var mockedMembershipService = new Mock<IMembershipService>();
            var mockedInviteService = new Mock<IInviteService>();
            var mockedGroupService = new Mock<IGroupService>();
            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();

            var controller = new MembershipController(mockedMembershipService.Object, mockedInviteService.Object, mockedGroupService.Object,
                mockedAuthenticationProvider.Object);

            // Act
            var result = await controller.AnswerInvite(groupName, username, true);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "groupName", "username")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "testGroup", "testUsername")]
        public async Task TestAnswerInvite_NoUser_ShouldReturnNotFound(int groupId,
            string userId, string groupName, string username)
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

            // Act
            var result = await controller.AnswerInvite(groupName, username, true);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "groupName", "username")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "testGroup", "testUsername")]
        public async Task TestAnswerInvite_ShouldCallInviteServiceIsUserInvited(int groupId,
            string userId, string groupName, string username)
        {
            // Arrange
            var mockedMembershipService = new Mock<IMembershipService>();
            var mockedInviteService = new Mock<IInviteService>();

            var group = new Group { Id = groupId };

            var mockedGroupService = new Mock<IGroupService>();
            mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

            var user = new User { Id = userId };

            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.FindByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);

            var controller = new MembershipController(mockedMembershipService.Object, mockedInviteService.Object, mockedGroupService.Object,
                mockedAuthenticationProvider.Object);

            // Act
            var result = await controller.AnswerInvite(groupName, username, true);

            // Assert
            mockedInviteService.Verify(s => s.IsUserInvited(groupId, userId), Times.Once);
        }

        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "groupName", "username")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "testGroup", "testUsername")]
        public async Task TestAnswerInvite_IsNotInvited_ShouldReturnForbidden(int groupId,
            string userId, string groupName, string username)
        {
            // Arrange
            var mockedMembershipService = new Mock<IMembershipService>();
            var mockedInviteService = new Mock<IInviteService>();

            var group = new Group { Id = groupId };

            var mockedGroupService = new Mock<IGroupService>();
            mockedGroupService.Setup(s => s.GetByName(It.IsAny<string>())).Returns(group);

            var user = new User { Id = userId };

            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.FindByUsernameAsync(It.IsAny<string>())).ReturnsAsync(user);

            var controller = new MembershipController(mockedMembershipService.Object, mockedInviteService.Object, mockedGroupService.Object,
                mockedAuthenticationProvider.Object);

            // Act
            var result = await controller.AnswerInvite(groupName, username, true);

            // Assert
            Assert.IsInstanceOf<ForbidResult>(result);
        }

        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "groupName", "username")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "testGroup", "testUsername")]
        public async Task TestAnswerInvite_AcceptsInvite_ShouldCallMembershipServiceJoinGroup(int groupId,
            string userId, string groupName, string username)
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

            var accepted = true;

            // Act
            var result = await controller.AnswerInvite(groupName, username, accepted);

            // Assert
            mockedMembershipService.Verify(s => s.JoinGroup(groupId, userId), Times.Once);
        }

        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "groupName", "username")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "testGroup", "testUsername")]
        public async Task TestAnswerInvite_AcceptsInvite_ShouldNotCallInviteServiceCancelInvite(int groupId,
            string userId, string groupName, string username)
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

            var accepted = true;

            // Act
            var result = await controller.AnswerInvite(groupName, username, accepted);

            // Assert
            mockedInviteService.Verify(s => s.CancelInvite(groupId, userId), Times.Never);
        }

        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "groupName", "username")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "testGroup", "testUsername")]
        public async Task TestAnswerInvite_DoesNotAcceptInvite_ShouldNotCallMembershipServiceJoinGroup(int groupId,
            string userId, string groupName, string username)
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

            var accepted = false;

            // Act
            var result = await controller.AnswerInvite(groupName, username, accepted);

            // Assert
            mockedMembershipService.Verify(s => s.JoinGroup(groupId, userId), Times.Never);
        }

        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "groupName", "username")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "testGroup", "testUsername")]
        public async Task TestAnswerInvite_DoesNotAcceptInvite_ShouldCallInviteServiceCancelInvite(int groupId,
            string userId, string groupName, string username)
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

            var accepted = false;

            // Act
            var result = await controller.AnswerInvite(groupName, username, accepted);

            // Assert
            mockedInviteService.Verify(s => s.CancelInvite(groupId, userId), Times.Once);
        }
    }
}
