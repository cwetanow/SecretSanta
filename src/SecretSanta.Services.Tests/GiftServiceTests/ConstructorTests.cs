using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using SecretSanta.Models;
using SecretSanta.Providers.Contracts;
using SecretSanta.Services.Contracts;

namespace SecretSanta.Services.Tests.GiftServiceTests
{
	[TestFixture]
	public class ConstructorTests
	{
		[Test]
		public void TestConstructor_ShouldInitializeCorrectly()
		{
			// Arrange
			var mockedRepository = new Mock<IRepository<Gift>>();
			var mockedUnitOfWork = new Mock<IUnitOfWork>();
			var mockedFactory = new Mock<IGiftFactory>();
			var mockedManager = new Mock<IGiftManager>();
			var mockedGroupService = new Mock<IGroupService>();

			// Act
			var service = new GiftService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedManager.Object, mockedGroupService.Object);

			// Assert
			Assert.IsNotNull(service);
			Assert.IsInstanceOf<IGiftService>(service);
		}
	}
}
