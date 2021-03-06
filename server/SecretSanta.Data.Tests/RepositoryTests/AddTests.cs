﻿using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Data.Tests.RepositoryTests.Fakes;

namespace SecretSanta.Data.Tests.RepositoryTests
{
    [TestFixture]
    public class AddTests
    {
        [Test]
        public void TestAdd_ShouldCallDbContextSetAdded()
        {
            // Arrange
            var mockedDbContext = new Mock<IDbContext>();

            var repository = new EfRepository<FakeRepositoryType>(mockedDbContext.Object);

            var entity = new Mock<FakeRepositoryType>();

            // Act
            repository.Add(entity.Object);

            // Assert
            mockedDbContext.Verify(c => c.SetAdded(entity.Object), Times.Once);
        }
    }
}
