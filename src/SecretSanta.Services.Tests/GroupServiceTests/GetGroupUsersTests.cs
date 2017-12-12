using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using SecretSanta.Models;

namespace SecretSanta.Services.Tests.GroupServiceTests
{
    [TestFixture]
    public class GetGroupUsersTests
    {
        [TestCase("group")]
        public void TestGetGroupUsers_ShouldCallRepositoryAll(string groupName)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Group>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupFactory>();
            var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();

            var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
                mockedGroupUserRepository.Object);

            // Act
            service.GetGroupUsers(groupName);

            // Assert
            mockedRepository.Verify(r => r.All, Times.Once);
        }

        [TestCase("group")]
        public void TestGetGroupUsers_NoGroupFound_ShouldReturnNull(string groupName)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Group>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupFactory>();
            var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();

            var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
                mockedGroupUserRepository.Object);

            // Act
            var result = service.GetGroupUsers(groupName);

            // Assert
            Assert.IsNull(result);
        }

        [TestCase("group")]
        public void TestGetGroupUsers_GroupFound_ShouldReturnCorrectly(string groupName)
        {
            // Arrange
            var users = new List<GroupUser>
            {
                new GroupUser
                {
                    User = new User()
                },
                new GroupUser
                {
                    User = new User()
                }
            };

            var group = new Group
            {
                GroupName = groupName,
                Users = users
            };

            var mockedRepository = new Mock<IRepository<Group>>();
            mockedRepository.Setup(r => r.All).Returns(new List<Group> { group }.AsQueryable());
            
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupFactory>();
            var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();

            var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
                mockedGroupUserRepository.Object);

            var expected = users
                .Select(g => g.User)
                .ToList();

            // Act
            var result = service.GetGroupUsers(groupName);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
