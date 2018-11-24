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
    public class CreateGroupAsyncTests
    {
        [TestCase("groupname", "id")]
        [TestCase("mygroup", "user id")]
        public async Task TestCreateGroupAsync_ShouldCallRepositoryAll(string groupName, string ownerId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Group>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupFactory>();
            var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();

            var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
                mockedGroupUserRepository.Object);

            // Act
            await service.CreateGroupAsync(groupName, ownerId);

            // Assert
            mockedRepository.Verify(r => r.All, Times.Once());
        }

        [TestCase("groupname", "id")]
        [TestCase("mygroup", "user id")]
        public async Task TestCreateGroupAsync_ThereIsGroup_ShouldReturnNull(string groupName, string ownerId)
        {
            // Arrange
            var group = new Group { GroupName = groupName };

            var mockedRepository = new Mock<IRepository<Group>>();
            mockedRepository.Setup(r => r.All)
                .Returns(new List<Group> { group }.AsQueryable());

            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupFactory>();
            var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();

            var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
                mockedGroupUserRepository.Object);

            // Act
            var result = await service.CreateGroupAsync(groupName, ownerId);

            // Assert
            Assert.IsNull(result);
        }

        [TestCase("groupname", "id")]
        [TestCase("mygroup", "user id")]
        public async Task TestCreateGroupAsync_ThereIsNoGroup_ShouldCallFactoryCreate(string groupName, string ownerId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Group>>();
            mockedRepository.Setup(r => r.All)
                .Returns(new List<Group>().AsQueryable());

            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupFactory>();
            var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();

            var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
                mockedGroupUserRepository.Object);

            // Act
            await service.CreateGroupAsync(groupName, ownerId);

            // Assert
            mockedFactory.Verify(f => f.CreateGroup(groupName, ownerId), Times.Once);
        }

        [TestCase("groupname", "id")]
        [TestCase("mygroup", "user id")]
        public async Task TestCreateGroupAsync_ThereIsNoGroup_ShouldCallRepositoryAdd(string groupName, string ownerId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Group>>();
            mockedRepository.Setup(r => r.All)
                .Returns(new List<Group>().AsQueryable());

            var mockedUnitOfWork = new Mock<IUnitOfWork>();

            var group = new Group();

            var mockedFactory = new Mock<IGroupFactory>();
            mockedFactory.Setup(f => f.CreateGroup(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(group);

            var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();

            var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
                mockedGroupUserRepository.Object);

            // Act
            await service.CreateGroupAsync(groupName, ownerId);

            // Assert
            mockedRepository.Verify(r => r.Add(group), Times.Once);
        }

        [TestCase("groupname", "id")]
        [TestCase("mygroup", "user id")]
        public async Task TestCreateGroupAsync_ThereIsNoGroup_ShouldUnitOfWorkCommitAsync(string groupName, string ownerId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Group>>();
            mockedRepository.Setup(r => r.All)
                .Returns(new List<Group>().AsQueryable());

            var mockedUnitOfWork = new Mock<IUnitOfWork>();

            var group = new Group();

            var mockedFactory = new Mock<IGroupFactory>();
            mockedFactory.Setup(f => f.CreateGroup(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(group);

            var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();

            var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
                mockedGroupUserRepository.Object);

            // Act
            await service.CreateGroupAsync(groupName, ownerId);

            // Assert
            mockedUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        [TestCase("groupname", "id")]
        [TestCase("mygroup", "user id")]
        public async Task TestCreateGroupAsync_ThereIsNoGroup_ShouldReturnCorrectly(string groupName, string ownerId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Group>>();
            mockedRepository.Setup(r => r.All)
                .Returns(new List<Group>().AsQueryable());

            var mockedUnitOfWork = new Mock<IUnitOfWork>();

            var group = new Group();

            var mockedFactory = new Mock<IGroupFactory>();
            mockedFactory.Setup(f => f.CreateGroup(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(group);

            var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();

            var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
                mockedGroupUserRepository.Object);

            // Act
            var result = await service.CreateGroupAsync(groupName, ownerId);

            // Assert
            Assert.AreSame(group, result);
        }
    }
}
