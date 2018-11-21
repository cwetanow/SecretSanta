using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using SecretSanta.Models;
using SecretSanta.Services.Contracts;

namespace SecretSanta.Services.Tests.MembershipServiceTests
{
    [TestFixture]
    public class ConstructorTests
    {
        [Test]
        public void TestConstructor_ShouldInitializeCorrectly()
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<GroupUser>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IGroupUserFactory>();

            // Act
            var service = new MembershipService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object);

            // Assert
            Assert.IsNotNull(service);
            Assert.IsInstanceOf<IMembershipService>(service);
        }
    }
}
