using NUnit.Framework;
using SecretSanta.Data.Contracts;
using Moq;
using System.Threading.Tasks;

namespace SecretSanta.Data.Tests.UnitOfWorkTests
{
    [TestFixture]
    public class CommitAsyncTests
    {
        [Test]
        public void TestCommitAsync_ShouldCallDbContextSaveChangesAsync()
        {
            // Arrange
            var mockedDbContext = new Mock<IDbContext>();

            var unitOfWork = new UnitOfWork(mockedDbContext.Object);

            // Act
            unitOfWork.CommitAsync();

            // Assert
            mockedDbContext.Verify(c => c.SaveChangesAsync(), Times.Once);
        }

        [TestCase(1)]
        [TestCase(723)]
        [TestCase(14)]
        public async Task TestCommitAsync_ShouldReturnCorrectly(int expectedResult)
        {
            // Arrange
            var mockedDbContext = new Mock<IDbContext>();
            mockedDbContext.Setup(d => d.SaveChangesAsync()).ReturnsAsync(expectedResult);

            var unitOfWork = new UnitOfWork(mockedDbContext.Object);

            // Act
            var result = await unitOfWork.CommitAsync();

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
