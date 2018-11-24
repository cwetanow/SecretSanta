using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using SecretSanta.Models;
using SecretSanta.Models.Enums;
using SecretSanta.Providers.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta.Services.Tests.InviteServiceTests
{
    [TestFixture]
    public class GetPendingInvitesTests
    {
        [TestCase(5, 0, true, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(7, 2, false, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void TestGetPendingInvites_ShouldCallRepositoryAll(int offset, int limit, bool sortAscending, string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Invite>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IInviteFactory>();
            var mockedDateTimeProvider = new Mock<IDateTimeProvider>();

            var service = new InviteService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedDateTimeProvider.Object);

            // Act
            service.GetPendingInvites(userId, sortAscending, limit, offset);

            // Assert
            mockedRepository.Verify(r => r.All, Times.Once);
        }

        [TestCase(5, 0, true, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(7, 2, false, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void TestGetPendingInvites_OrderByAscending_ShouldOrderCorrectly(int offset, int limit, bool sortAscending, string userId)
        {
            // Arrange
            var invites = new List<Invite>
            {
                new Invite{State=InviteState.Accepted, UserId=userId},
                new Invite{State=InviteState.Declined, UserId=string.Empty}
            }
            .AsQueryable();

            var mockedRepository = new Mock<IRepository<Invite>>();
            mockedRepository.Setup(r => r.All)
                .Returns(invites);

            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IInviteFactory>();
            var mockedDateTimeProvider = new Mock<IDateTimeProvider>();

            var service = new InviteService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedDateTimeProvider.Object);

            var expectedInvites = invites
                 .Where(i => i.State == InviteState.Pending && i.UserId.Equals(userId))
                 .OrderBy(i => i.Date);

            // Act
            var result = service.GetPendingInvites(userId, sortAscending, limit, offset);

            // Assert
            CollectionAssert.AreEqual(expectedInvites, result);
        }

        [TestCase(5, 0, true, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(7, 2, false, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void TestGetPendingInvites_OrderByDescending_ShouldOrderCorrectly(int offset, int limit, bool sortAscending, string userId)
        {
            // Arrange
            var invites = new List<Invite>
                {
                    new Invite{State=InviteState.Accepted, UserId=userId},
                    new Invite{State=InviteState.Declined, UserId=string.Empty}
                }
                .AsQueryable();

            var mockedRepository = new Mock<IRepository<Invite>>();
            mockedRepository.Setup(r => r.All)
                .Returns(invites);

            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IInviteFactory>();
            var mockedDateTimeProvider = new Mock<IDateTimeProvider>();

            var service = new InviteService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedDateTimeProvider.Object);

            var expectedInvites = invites
                .Where(i => i.State == InviteState.Pending && i.UserId.Equals(userId))
                .OrderByDescending(i => i.Date);

            // Act
            var result = service.GetPendingInvites(userId, sortAscending, limit, offset);

            // Assert
            CollectionAssert.AreEqual(expectedInvites, result);
        }

        [TestCase(5, 2, true, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(3, 2, false, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void TestGetPendingInvites_ShouldPageCorrectly(int offset, int limit, bool sortAscending, string userId)
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
            var mockedDateTimeProvider = new Mock<IDateTimeProvider>();

            var service = new InviteService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedDateTimeProvider.Object);

            var expectedInvites = invites
                .Where(i => i.State == InviteState.Pending && i.UserId.Equals(userId))
                .OrderBy(i => i.Date)
                .Skip(offset)
                .Take(limit);

            // Act
            var result = service.GetPendingInvites(userId, sortAscending, limit, offset);

            // Assert
            CollectionAssert.AreEqual(expectedInvites, result);
        }
    }
}
