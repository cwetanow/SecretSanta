using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using SecretSanta.Models;
using SecretSanta.Providers.Contracts;
using SecretSanta.Services.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta.Services.Tests.GiftServiceTests
{
	[TestFixture]
	public class GetGiftInGroupTests
	{
		[TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public void TestGetGiftInGroup_ShouldCallRepositoryAll(int groupId, string senderId)
		{
			// Arrange
			var mockedRepository = new Mock<IRepository<Gift>>();
			var mockedUnitOfWork = new Mock<IUnitOfWork>();
			var mockedFactory = new Mock<IGiftFactory>();
			var mockedManager = new Mock<IGiftManager>();
			var mockedGroupService = new Mock<IGroupService>();

			var service = new GiftService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedManager.Object, mockedGroupService.Object);

			// Act
			service.GetGiftInGroup(groupId, senderId);

			// Assert
			mockedRepository.Verify(r => r.All, Times.Once);
		}

		[TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		[TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		public void TestGetGiftInGroup_ShouldReturnCorrectly(int groupId, string senderId)
		{
			// Arrange
			var gift = new Gift { GroupId = groupId, SenderId = senderId };

			var gifts = new List<Gift> { gift }
			.AsQueryable();

			var mockedRepository = new Mock<IRepository<Gift>>();
			mockedRepository.Setup(r => r.All).Returns(gifts);

			var mockedUnitOfWork = new Mock<IUnitOfWork>();
			var mockedFactory = new Mock<IGiftFactory>();
			var mockedManager = new Mock<IGiftManager>();
			var mockedGroupService = new Mock<IGroupService>();

			var service = new GiftService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedManager.Object, mockedGroupService.Object);

			// Act
			var result = service.GetGiftInGroup(groupId, senderId);

			// Assert
			Assert.AreSame(gift, result);
		}
	}
}
