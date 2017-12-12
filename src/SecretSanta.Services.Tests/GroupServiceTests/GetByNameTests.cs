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
    public class GetByNameTests
    {
        [TestCase("groupname")]
        [TestCase("mygroup")]
        public void TestGetByName_ShouldCallRepositoryAllCorrectly(string name)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Group>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupFactory>();
            var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();

            var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
                mockedGroupUserRepository.Object);

            // Act
            service.GetByName(name);

            // Assert
            mockedRepository.Verify(r => r.All, Times.Once());
        }

        [TestCase("groupname")]
        [TestCase("mygroup")]
        public void TestGetByName_ShouldReturnCorrectly(string name)
        {
            // Arrange
            var group = new Group { GroupName = name };

            var mockedRepository = new Mock<IRepository<Group>>();
            mockedRepository.Setup(r => r.All)
                .Returns(new List<Group> { group }.AsQueryable());

            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupFactory>();
            var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();

            var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
                mockedGroupUserRepository.Object);

            // Act
            var result = service.GetByName(name);

            // Assert
            Assert.AreSame(group, result);
        }

        [TestCase("groupname")]
        [TestCase("mygroup")]
        public void TestGetByName_NoGroup_ShouldReturnNull(string name)
        {
            // Arrange
            var group = new Group { GroupName = string.Empty };

            var mockedRepository = new Mock<IRepository<Group>>();
            mockedRepository.Setup(r => r.All)
                .Returns(new List<Group> { group }.AsQueryable());

            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupFactory>();
            var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();

            var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
                mockedGroupUserRepository.Object);

            // Act
            var result = service.GetByName(name);

            // Assert
            Assert.IsNull(result);
        }
    }
}
