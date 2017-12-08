using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using SecretSanta.Models;
using SecretSanta.Models.Enums;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta.Services.Tests.InviteServiceTests
{
    [TestFixture]
    public class GetPendingInvitesTests
    {
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void TestGetPendingInvites_ShouldCallRepositoryAll(string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Invite>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IInviteFactory>();

            var service = new InviteService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object);

            // Act
            service.GetPendingInvites(userId);

            // Assert
            mockedRepository.Verify(r => r.All, Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void TestGetPendingInvites_ShouldReturnCorrectly(string userId)
        {
            // Arrange
            var invites = new List<Invite>
            {
                new Invite{State=InviteState.Accepted, UserId=userId},
                new Invite{State=InviteState.Declined, UserId=string.Empty},
                new Invite{State=InviteState.Accepted, UserId=userId},
                new Invite{State=InviteState.Pending, UserId=string.Empty},
                new Invite{State=InviteState.Pending, UserId=userId},
                new Invite{State=InviteState.Pending, UserId=userId}
            }
            .AsQueryable();

            var mockedRepository = new Mock<IRepository<Invite>>();
            mockedRepository.Setup(r => r.All)
                .Returns(invites);

            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IInviteFactory>();

            var service = new InviteService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object);

            var expectedInvites = invites
                 .Where(i => i.State == InviteState.Pending && i.UserId.Equals(userId));

            // Act
            var result = service.GetPendingInvites(userId);

            // Assert
            CollectionAssert.AreEqual(expectedInvites, result);
        }
    }
}
