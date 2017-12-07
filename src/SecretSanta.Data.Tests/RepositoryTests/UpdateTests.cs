using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Data.Tests.RepositoryTests.Fakes;

namespace SecretSanta.Data.Tests.RepositoryTests
{
    [TestFixture]
    public class UpdateTests
    {
        [Test]
        public void TestUpdate_ShouldCallDbContextSetUpdated()
        {
            // Arrange
            var mockedDbContext = new Mock<IDbContext>();

            var repository = new EfRepository<FakeRepositoryType>(mockedDbContext.Object);

            var entity = new Mock<FakeRepositoryType>();

            // Act
            repository.Update(entity.Object);

            // Assert
            mockedDbContext.Verify(c => c.SetUpdated(entity.Object), Times.Once);
        }
    }
}
