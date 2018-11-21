using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Data.Tests.RepositoryTests.Fakes;
using System;

namespace SecretSanta.Data.Tests.RepositoryTests
{
    [TestFixture]
    public class ConstructorTests
    {
        [Test]
        public void TestConstructor_PassDbContextNull_ShouldThrowArgumentNullException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => new EfRepository<FakeRepositoryType>(null));
        }

        [Test]
        public void TestConstructor_PassDbContextCorrectly_ShouldNotThrow()
        {
            // Arrange
            var mockedDbContext = new Mock<IDbContext>();

            // Act, Assert
            Assert.DoesNotThrow(() => new EfRepository<FakeRepositoryType>(mockedDbContext.Object));
        }

        [Test]
        public void TestConstructor_PassDbContextCorrectly_ShouldInitializeCorrectly()
        {
            // Arrange
            var mockedDbContext = new Mock<IDbContext>();

            // Act
            var repository = new EfRepository<FakeRepositoryType>(mockedDbContext.Object);

            // Assert
            Assert.IsNotNull(repository);
        }
    }
}
