using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using SecretSanta.Models;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta.Services.Tests.GroupServiceTests
{
    [TestFixture]
    public class GetUserGroupsTests
    {
        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void TestGetUserGroups_ShouldCallRepositoryAll(string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Group>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupFactory>();
            var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();

            var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
                mockedGroupUserRepository.Object);

            // Act
            service.GetUserGroups(userId);

            // Assert
            mockedGroupUserRepository.Verify(r => r.All, Times.Once);
        }

        [TestCase("d547a40d-c45f-4c43-99de-0bfe9199ff95")]
        [TestCase("99ae8dd3-1067-4141-9675-62e94bb6caaa")]
        public void TestGetUserGroups_ShouldReturnCorrectly(string userId)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Group>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupFactory>();

            var groups = new List<GroupUser>
            {
                new GroupUser{UserId=userId, Group=new Group()},
                new GroupUser{UserId=string.Empty, Group=new Group()}
            };

            var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();
            mockedGroupUserRepository.Setup(r => r.All).Returns(groups.AsQueryable());

            var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
                mockedGroupUserRepository.Object);

            var expectedResult = groups
                .Where(g => g.UserId.Equals(userId))
                .Select(g => g.Group)
                .ToList();

            // Act
            var result = service.GetUserGroups(userId);

            // Assert
            CollectionAssert.AreEqual(expectedResult, result);
        }
    }
}
