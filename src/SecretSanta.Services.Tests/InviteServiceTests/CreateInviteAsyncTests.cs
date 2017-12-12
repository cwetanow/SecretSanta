using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using SecretSanta.Models;
using SecretSanta.Providers.Contracts;
using System;
using System.Threading.Tasks;

namespace SecretSanta.Services.Tests.InviteServiceTests
{
    [TestFixture]
    public class CreateInviteAsyncTests
    {
        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestCreateInviteAsync_ShouldCallDateTimeProviderGetCurrentTime(int groupId, string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Invite>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IInviteFactory>();
            var mockedDateTimeProvider = new Mock<IDateTimeProvider>();

            var service = new InviteService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedDateTimeProvider.Object);

            // Act
            await service.CreateInviteAsync(groupId, userId);

            // Assert
            mockedDateTimeProvider.Verify(p => p.GetCurrentTime(), Times.Once);
        }

        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestCreateInviteAsync_ShouldCallFactoryCreate(int groupId, string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Invite>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IInviteFactory>();

            var date = new DateTime();
            var mockedDateTimeProvider = new Mock<IDateTimeProvider>();
            mockedDateTimeProvider.Setup(p => p.GetCurrentTime()).Returns(date);

            var service = new InviteService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedDateTimeProvider.Object);

            // Act
            await service.CreateInviteAsync(groupId, userId);

            // Assert
            mockedFactory.Verify(f => f.CreateInvite(groupId, userId, date), Times.Once);
        }

        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestCreateInviteAsync_ShouldCallRepositoryAdd(int groupId, string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Invite>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();

            var invite = new Invite();
            var mockedFactory = new Mock<IInviteFactory>();
            mockedFactory.Setup(f => f.CreateInvite(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(invite);

            var mockedDateTimeProvider = new Mock<IDateTimeProvider>();

            var service = new InviteService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedDateTimeProvider.Object);

            // Act
            await service.CreateInviteAsync(groupId, userId);

            // Assert
            mockedRepository.Verify(r => r.Add(invite), Times.Once);
        }

        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestCreateInviteAsync_ShouldCallUnitOfWorkCommitAsync(int groupId, string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Invite>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();

            var invite = new Invite();
            var mockedFactory = new Mock<IInviteFactory>();
            mockedFactory.Setup(f => f.CreateInvite(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(invite);

            var mockedDateTimeProvider = new Mock<IDateTimeProvider>();

            var service = new InviteService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedDateTimeProvider.Object);

            // Act
            await service.CreateInviteAsync(groupId, userId);

            // Assert
            mockedUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestCreateInviteAsync_ShouldReturnCorrectly(int groupId, string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Invite>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();

            var invite = new Invite();
            var mockedFactory = new Mock<IInviteFactory>();
            mockedFactory.Setup(f => f.CreateInvite(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(invite);

            var mockedDateTimeProvider = new Mock<IDateTimeProvider>();

            var service = new InviteService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedDateTimeProvider.Object);

            // Act
            var result = await service.CreateInviteAsync(groupId, userId);

            // Assert
            Assert.AreSame(invite, result);
        }
    }
}
