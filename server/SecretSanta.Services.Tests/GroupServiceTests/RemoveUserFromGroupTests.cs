using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using SecretSanta.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Services.Tests.GroupServiceTests
{
    [TestFixture]
    public class RemoveUserFromGroupTests
    {
        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestRemoveUserFromGroup_ShouldCallRepositoryAll(int groupId, string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Group>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupFactory>();
            var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();

            var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
                mockedGroupUserRepository.Object);

            // Act
            await service.RemoveUserFromGroup(groupId, userId);

            // Assert
            mockedGroupUserRepository.Verify(r => r.All, Times.Once);
        }

        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestRemoveUserFromGroup_NoUserInGroup_ShouldNotCallRepositoryDelete(int groupId, string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Group>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupFactory>();
            var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();

            var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
                mockedGroupUserRepository.Object);

            // Act
            await service.RemoveUserFromGroup(groupId, userId);

            // Assert
            mockedGroupUserRepository.Verify(r => r.Delete(It.IsAny<GroupUser>()), Times.Never);
        }

        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestRemoveUserFromGroup_ThereIsUser_ShouldCallRepositoryDelete(int groupId, string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Group>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupFactory>();

            var groupUser = new GroupUser { UserId = userId, GroupId = groupId };

            var groupUsers = new List<GroupUser> { groupUser };

            var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();
            mockedGroupUserRepository.Setup(r => r.All).Returns(groupUsers.AsQueryable());

            var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
                mockedGroupUserRepository.Object);

            // Act
            await service.RemoveUserFromGroup(groupId, userId);

            // Assert
            mockedGroupUserRepository.Verify(r => r.Delete(groupUser), Times.Once);
        }

        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestRemoveUserFromGroup_ThereIsUser_ShouldCallUnitOfWorkCommitAsync(int groupId, string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Group>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupFactory>();

            var groupUser = new GroupUser { UserId = userId, GroupId = groupId };

            var groupUsers = new List<GroupUser> { groupUser };

            var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();
            mockedGroupUserRepository.Setup(r => r.All).Returns(groupUsers.AsQueryable());

            var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
                mockedGroupUserRepository.Object);

            // Act
            await service.RemoveUserFromGroup(groupId, userId);

            // Assert
            mockedUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }
    }
}