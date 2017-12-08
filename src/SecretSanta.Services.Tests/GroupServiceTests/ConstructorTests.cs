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
        public void TestConstructor_PassRepositoryNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupFactory>();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => new GroupService(null, mockedUnitOfWork.Object, mockedFactory.Object));
        }

        [Test]
        public void TestConstructor_PassUnitOfWorkNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Group>>();
            var mockedFactory = new Mock<IGroupFactory>();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => new GroupService(mockedRepository.Object, null, mockedFactory.Object));
        }

        [Test]
        public void TestConstructor_PassFactoryNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Group>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, null));
        }

        [Test]
        public void TestConstructor_PassEverythingCorrectly_ShouldNotThrow()
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Group>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupFactory>();

            // Act, Assert
            Assert.DoesNotThrow(() => new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object));
        }

        [Test]
        public void TestConstructor_PassEverythingCorrectly_ShouldInitializeCorrectly()
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Group>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupFactory>();

            // Act
            var service = new GroupService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object);

            // Assert
            Assert.IsNotNull(service);
        }
    }
}
