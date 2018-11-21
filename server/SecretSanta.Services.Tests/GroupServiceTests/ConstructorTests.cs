using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using SecretSanta.Models;
using System;

namespace SecretSanta.Services.Tests.GroupServiceTests
{
    [TestFixture]
    public class ConstructorTests
    {
        [Test]
        public void TestConstructor_PassEverythingCorrectly_ShouldInitializeCorrectly()
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Group>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupFactory>();
            var mockedGroupUserRepository = new Mock<IRepository<GroupUser>>();

            // Act
            var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object,
                mockedGroupUserRepository.Object);

            // Assert
            Assert.IsNotNull(service);
        }
    }
}
