using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using System;

namespace SecretSanta.Data.Tests.UnitOfWorkTests
{
    [TestFixture]
    public class ConstructorTests
    {
        [Test]
        public void TestConstructor_PassDbContextNull_ShouldThrowArgumentNullException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => new UnitOfWork(null));
        }

        [Test]
        public void TestConstructor_PassDbContextCorrectly_ShouldNotThrow()
        {
            // Arrange
            var mockedDbContext = new Mock<IDbContext>();

            // Act, Assert
            Assert.DoesNotThrow(() => new UnitOfWork(mockedDbContext.Object));
        }

        [Test]
        public void TestConstructor_PassDbContextCorrectly_ShouldInitializeCorrectly()
        {
            // Arrange
            var mockedDbContext = new Mock<IDbContext>();

            // Act
            var unitOfWork = new UnitOfWork(mockedDbContext.Object);

            // Assert
            Assert.IsNotNull(unitOfWork);
        }
    }
}
