using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Models;
using System;

namespace SecretSanta.Services.Tests.UserServiceTests
{
    [TestFixture]
    public class ConstructorTests
    {
        [Test]
        public void TestConstructor_PassRepositoryNull_ShouldThrowArgumentNullException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => new UserService(null));
        }

        [Test]
        public void TestConstructor_PassEverythingCorrectly_ShouldNotThrow()
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<User>>();

            // Act, Assert
            Assert.DoesNotThrow(() => new UserService(mockedRepository.Object));
        }

        [Test]
        public void TestConstructor_PassEverythingCorrectly_ShouldInitializeCorrectly()
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<User>>();

            // Act
            var service = new UserService(mockedRepository.Object);

            // Assert
            Assert.IsNotNull(service);
        }
    }
}
