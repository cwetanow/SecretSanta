using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Models;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Controllers;
using SecretSanta.Web.Infrastructure;
using SecretSanta.Web.Models.Invite;

namespace SecretSanta.Web.Tests.Controllers.InviteControllerTests
{
    [TestFixture]
    public class GetPendingUserInvitesTests
    {
        [TestCase(5, 0, true, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(7, 2, false, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestGetPendingUserInvites_ShouldCallAuthenticationProviderGetCurrentUserAsync(int offset,
            int limit, bool sortAscending, string userId)
        {
            // Arrange
            var mockedService = new Mock<IInviteService>();
            var mockedFactory = new Mock<IDtoFactory>();

            var user = new User { Id = userId };

            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

            var controller = new InviteController(mockedAuthenticationProvider.Object,
                mockedService.Object, mockedFactory.Object);

            // Act
            await controller.GetPendingUserInvites(offset, limit, sortAscending);

            // Assert
            mockedAuthenticationProvider.Verify(p => p.GetCurrentUserAsync(), Times.Once);
        }

        [TestCase(5, 0, true, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(7, 2, false, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestGetPendingUserInvites_ShouldCallServiceGetPendingInvites(int offset,
            int limit, bool sortAscending, string userId)
        {
            // Arrange
            var mockedService = new Mock<IInviteService>();
            var mockedFactory = new Mock<IDtoFactory>();

            var user = new User { Id = userId };

            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

            var controller = new InviteController(mockedAuthenticationProvider.Object,
                mockedService.Object, mockedFactory.Object);

            // Act
            await controller.GetPendingUserInvites(offset, limit, sortAscending);

            // Assert
            mockedService.Verify(s => s.GetPendingInvites(userId, sortAscending, limit, offset), Times.Once);
        }

        [TestCase(5, 0, true, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(7, 2, false, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestGetPendingUserInvites_ShouldCallFactoryCreate(int offset,
            int limit, bool sortAscending, string userId)
        {
            // Arrange
            var invites = new List<Invite>
            {
                new Invite(),
                new Invite()
            };

            var mockedService = new Mock<IInviteService>();
            mockedService.Setup(s =>
                    s.GetPendingInvites(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(invites);

            var mockedFactory = new Mock<IDtoFactory>();

            var user = new User { Id = userId };

            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

            var controller = new InviteController(mockedAuthenticationProvider.Object,
                mockedService.Object, mockedFactory.Object);

            // Act
            await controller.GetPendingUserInvites(offset, limit, sortAscending);

            // Assert
            mockedFactory.Verify(f => f.CreateInviteListDto(invites), Times.Once);
        }

        [TestCase(5, 0, true, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(7, 2, false, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestGetPendingUserInvites_ShouldReturnOk(int offset,
            int limit, bool sortAscending, string userId)
        {
            // Arrange
            var mockedService = new Mock<IInviteService>();

            var dto = new InviteListDto();
            var mockedFactory = new Mock<IDtoFactory>();
            mockedFactory.Setup(f => f.CreateInviteListDto(It.IsAny<IEnumerable<Invite>>()))
                .Returns(dto);

            var user = new User { Id = userId };

            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

            var controller = new InviteController(mockedAuthenticationProvider.Object,
                mockedService.Object, mockedFactory.Object);

            // Act
            var result = await controller.GetPendingUserInvites(offset, limit, sortAscending);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase(5, 0, true, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(7, 2, false, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestGetPendingUserInvites_ShouldSetCorrectBody(int offset,
            int limit, bool sortAscending, string userId)
        {
            // Arrange
            var mockedService = new Mock<IInviteService>();

            var dto = new InviteListDto();
            var mockedFactory = new Mock<IDtoFactory>();
            mockedFactory.Setup(f => f.CreateInviteListDto(It.IsAny<IEnumerable<Invite>>()))
                .Returns(dto);

            var user = new User { Id = userId };

            var mockedAuthenticationProvider = new Mock<IAuthenticationProvider>();
            mockedAuthenticationProvider.Setup(p => p.GetCurrentUserAsync()).ReturnsAsync(user);

            var controller = new InviteController(mockedAuthenticationProvider.Object,
                mockedService.Object, mockedFactory.Object);

            // Act
            var result = await controller.GetPendingUserInvites(offset, limit, sortAscending) as OkObjectResult;

            // Assert
            Assert.AreSame(dto, result.Value);
        }
    }
}
