using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using SecretSanta.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Services.Tests.MembershipServiceTests
{
    [TestFixture]
    public class JoinGroupTests
    {
        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestJoinGroup_ShouldCallRepositoryAll(int groupId, string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<GroupUser>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupUserFactory>();

            var service = new MembershipService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object);

            // Act
            await service.JoinGroup(groupId, userId);

            // Assert
            mockedRepository.Verify(r => r.All, Times.Once);
        }

        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestJoinGroup_IsAlreadyMember_ShouldReturnNull(int groupId, string userId)
        {
            // Arrange
            var groupUser = new GroupUser { GroupId = groupId, UserId = userId };

            var mockedRepository = new Mock<IRepository<GroupUser>>();
            mockedRepository.Setup(r => r.All).Returns(new List<GroupUser> { groupUser }.AsQueryable());

            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupUserFactory>();

            var service = new MembershipService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object);

            // Act
            var result = await service.JoinGroup(groupId, userId);

            // Assert
            Assert.IsNull(result);
        }

        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestJoinGroup_IsNotAMember_ShouldCallFactoryCreate(int groupId, string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<GroupUser>>();

            var mockedUnitOfWork = new Mock<IUnitOfWork>();

            var groupUser = new GroupUser { GroupId = groupId, UserId = userId };

            var mockedFactory = new Mock<IGroupUserFactory>();

            var service = new MembershipService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object);

            // Act
            var result = await service.JoinGroup(groupId, userId);

            // Assert
            mockedFactory.Verify(f => f.CreateGroupUser(groupId, userId), Times.Once);
        }

        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestJoinGroup_ShouldCallRepositoryAdd(int groupId, string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<GroupUser>>();

            var mockedUnitOfWork = new Mock<IUnitOfWork>();

            var groupUser = new GroupUser { GroupId = groupId, UserId = userId };

            var mockedFactory = new Mock<IGroupUserFactory>();
            mockedFactory.Setup(f => f.CreateGroupUser(It.IsAny<int>(), It.IsAny<string>())).Returns(groupUser);

            var service = new MembershipService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object);

            // Act
            var result = await service.JoinGroup(groupId, userId);

            // Assert
            mockedRepository.Verify(r => r.Add(groupUser), Times.Once);
        }

        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestJoinGroup_ShouldCallUnitOfWorkCommitAsync(int groupId, string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<GroupUser>>();

            var mockedUnitOfWork = new Mock<IUnitOfWork>();

            var groupUser = new GroupUser { GroupId = groupId, UserId = userId };

            var mockedFactory = new Mock<IGroupUserFactory>();
            mockedFactory.Setup(f => f.CreateGroupUser(It.IsAny<int>(), It.IsAny<string>())).Returns(groupUser);

            var service = new MembershipService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object);

            // Act
            var result = await service.JoinGroup(groupId, userId);

            // Assert
            mockedUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        [TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public async Task TestJoinGroup_ShouldReturnCorrectly(int groupId, string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<GroupUser>>();

            var mockedUnitOfWork = new Mock<IUnitOfWork>();

            var groupUser = new GroupUser { GroupId = groupId, UserId = userId };

            var mockedFactory = new Mock<IGroupUserFactory>();
            mockedFactory.Setup(f => f.CreateGroupUser(It.IsAny<int>(), It.IsAny<string>())).Returns(groupUser);

            var service = new MembershipService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object);

            // Act
            var result = await service.JoinGroup(groupId, userId);

            // Assert
            Assert.AreSame(groupUser, result);
        }
    }
}
