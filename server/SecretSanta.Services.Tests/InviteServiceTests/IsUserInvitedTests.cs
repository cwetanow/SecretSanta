using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using SecretSanta.Models;
using SecretSanta.Providers.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta.Services.Tests.InviteServiceTests
{
    [TestFixture]
    public class IsUserInvitedTests
    {
        [TestCase(5, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(7, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void TestIsUserInvited_ShouldCallRepositoryAll(int groupId, string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Invite>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IInviteFactory>();
            var mockedDateTimeProvider = new Mock<IDateTimeProvider>();

            var service = new InviteService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedDateTimeProvider.Object);

            // Act
            service.IsUserInvited(groupId, userId);

            // Assert
            mockedRepository.Verify(r => r.All, Times.Once);
        }

        [TestCase(5, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(7, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void TestIsUserInvited_NoInvite_ShouldReturnFalse(int groupId, string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Invite>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IInviteFactory>();
            var mockedDateTimeProvider = new Mock<IDateTimeProvider>();

            var service = new InviteService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedDateTimeProvider.Object);

            // Act
            var result = service.IsUserInvited(groupId, userId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestCase(5, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(7, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void TestIsUserInvited_ThereIsInvite_ShouldReturnTrue(int groupId, string userId)
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
            var result = service.IsUserInvited(groupId, userId);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
