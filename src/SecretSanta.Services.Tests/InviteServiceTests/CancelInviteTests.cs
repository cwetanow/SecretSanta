using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using SecretSanta.Models;
using SecretSanta.Providers.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Services.Tests.InviteServiceTests
{
    class CancelInviteTests
    {
        [TestCase(5, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(7, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestCancelInvite_ShouldCallRepositoryAll(int groupId, string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Invite>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IInviteFactory>();
            var mockedDateTimeProvider = new Mock<IDateTimeProvider>();

            var service = new InviteService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedDateTimeProvider.Object);

            // Act
            await service.CancelInvite(groupId, userId);

            // Assert
            mockedRepository.Verify(r => r.All, Times.Once);
        }

        [TestCase(5, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(7, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestCancelInvite_NoInvite_ShouldNotCallRepositoryDelete(int groupId, string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Invite>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IInviteFactory>();
            var mockedDateTimeProvider = new Mock<IDateTimeProvider>();

            var service = new InviteService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedDateTimeProvider.Object);

            // Act
            await service.CancelInvite(groupId, userId);

            // Assert
            mockedRepository.Verify(r => r.Delete(It.IsAny<Invite>()), Times.Never);
        }

        [TestCase(5, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(7, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestCancelInvite_ThereIsInvite_ShouldCallRepositoryDelete(int groupId, string userId)
        {
            // Arrange
            var invite = new Invite { GroupId = groupId, UserId = userId };

            var mockedRepository = new Mock<IRepository<Invite>>();
            mockedRepository.Setup(r => r.All).Returns(new List<Invite> { invite }.AsQueryable());

            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IInviteFactory>();
            var mockedDateTimeProvider = new Mock<IDateTimeProvider>();

            var service = new InviteService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedDateTimeProvider.Object);

            // Act
            await service.CancelInvite(groupId, userId);

            // Assert
            mockedRepository.Verify(r => r.Delete(invite), Times.Once);
        }

        [TestCase(5, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(7, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestCancelInvite_ThereIsInvite_ShouldCallUnitOfWorkCommitAsync(int groupId, string userId)
        {
            // Arrange
            var invite = new Invite { GroupId = groupId, UserId = userId };

            var mockedRepository = new Mock<IRepository<Invite>>();
            mockedRepository.Setup(r => r.All).Returns(new List<Invite> { invite }.AsQueryable());

            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IInviteFactory>();
            var mockedDateTimeProvider = new Mock<IDateTimeProvider>();

            var service = new InviteService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedDateTimeProvider.Object);

            // Act
            await service.CancelInvite(groupId, userId);

            // Assert
            mockedUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }
    }
}
