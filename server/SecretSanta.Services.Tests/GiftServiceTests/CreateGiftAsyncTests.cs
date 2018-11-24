using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using SecretSanta.Models;
using SecretSanta.Providers.Contracts;
using SecretSanta.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta.Services.Tests.GiftServiceTests
{
	[TestFixture]
	public class CreateGiftAsyncTests
	{
		[TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		[TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		public async Task TestCreateGiftAsync_ShouldCallFactoryCreate(int groupId, string senderId, string receiverId)
		{
			// Arrange
			var sender = new User { Id = senderId, SentGifts = new List<Gift>() };
			var receiver = new User { Id = receiverId, ReceivedGifts = new List<Gift>() };

			var mockedRepository = new Mock<IRepository<Gift>>();
			var mockedUnitOfWork = new Mock<IUnitOfWork>();
			var mockedFactory = new Mock<IGiftFactory>();
			var mockedManager = new Mock<IGiftManager>();
			var mockedGroupService = new Mock<IGroupService>();

			var service = new GiftService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedManager.Object, mockedGroupService.Object);

			// Act
			await service.CreateGiftAsync(groupId, sender, receiver);

			// Assert
			mockedFactory.Verify(f => f.CreateGift(groupId, senderId, receiverId), Times.Once);
		}

		[TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		[TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		public async Task TestCreateGiftAsync_ShouldCallRepositoryAdd(int groupId, string senderId, string receiverId)
		{
			// Arrange
			var sender = new User { Id = senderId, SentGifts = new List<Gift>() };
			var receiver = new User { Id = receiverId, ReceivedGifts = new List<Gift>() };

			var mockedRepository = new Mock<IRepository<Gift>>();
			var mockedUnitOfWork = new Mock<IUnitOfWork>();

			var gift = new Gift();

			var mockedFactory = new Mock<IGiftFactory>();
			mockedFactory.Setup(f => f.CreateGift(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(gift);

			var mockedManager = new Mock<IGiftManager>();
			var mockedGroupService = new Mock<IGroupService>();

			var service = new GiftService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedManager.Object, mockedGroupService.Object);

			// Act
			await service.CreateGiftAsync(groupId, sender, receiver);

			// Assert
			mockedRepository.Verify(r => r.Add(gift), Times.Once);
		}

		[TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		[TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		public async Task TestCreateGiftAsync_ShouldCallUnitOfWorkCommitAsyncTwoTimes(int groupId, string senderId, string receiverId)
		{
			// Arrange
			var sender = new User { Id = senderId, SentGifts = new List<Gift>() };
			var receiver = new User { Id = receiverId, ReceivedGifts = new List<Gift>() };

			var mockedRepository = new Mock<IRepository<Gift>>();
			var mockedUnitOfWork = new Mock<IUnitOfWork>();

			var gift = new Gift();

			var mockedFactory = new Mock<IGiftFactory>();
			mockedFactory.Setup(f => f.CreateGift(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(gift);

			var mockedManager = new Mock<IGiftManager>();
			var mockedGroupService = new Mock<IGroupService>();

			var service = new GiftService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedManager.Object, mockedGroupService.Object);

			// Act
			await service.CreateGiftAsync(groupId, sender, receiver);

			// Assert
			mockedUnitOfWork.Verify(u => u.CommitAsync(), Times.Exactly(2));
		}

		[TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		[TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		public async Task TestCreateGiftAsync_ShouldAddGiftToSenderSentGifts(int groupId, string senderId, string receiverId)
		{
			// Arrange
			var sender = new User { Id = senderId, SentGifts = new List<Gift>() };
			var receiver = new User { Id = receiverId, ReceivedGifts = new List<Gift>() };

			var mockedRepository = new Mock<IRepository<Gift>>();
			var mockedUnitOfWork = new Mock<IUnitOfWork>();

			var gift = new Gift();

			var mockedFactory = new Mock<IGiftFactory>();
			mockedFactory.Setup(f => f.CreateGift(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(gift);

			var mockedManager = new Mock<IGiftManager>();
			var mockedGroupService = new Mock<IGroupService>();

			var service = new GiftService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedManager.Object, mockedGroupService.Object);

			// Act
			await service.CreateGiftAsync(groupId, sender, receiver);

			// Assert
			CollectionAssert.Contains(sender.SentGifts, gift);
		}

		[TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		[TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		public async Task TestCreateGiftAsync_ShouldAddGiftToReceiverReceivedGifts(int groupId, string senderId, string receiverId)
		{
			// Arrange
			var sender = new User { Id = senderId, SentGifts = new List<Gift>() };
			var receiver = new User { Id = receiverId, ReceivedGifts = new List<Gift>() };

			var mockedRepository = new Mock<IRepository<Gift>>();
			var mockedUnitOfWork = new Mock<IUnitOfWork>();

			var gift = new Gift();

			var mockedFactory = new Mock<IGiftFactory>();
			mockedFactory.Setup(f => f.CreateGift(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(gift);

			var mockedManager = new Mock<IGiftManager>();
			var mockedGroupService = new Mock<IGroupService>();

			var service = new GiftService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedManager.Object, mockedGroupService.Object);

			// Act
			await service.CreateGiftAsync(groupId, sender, receiver);

			// Assert
			CollectionAssert.Contains(sender.SentGifts, gift);
		}

		[TestCase(2, "d547a40d-c45f-4c43-99de-0bfe9199ff95", "99ae8dd3-1067-4141-9675-62e94bb6caaa")]
		[TestCase(5, "99ae8dd3-1067-4141-9675-62e94bb6caaa", "d547a40d-c45f-4c43-99de-0bfe9199ff95")]
		public async Task TestCreateGiftAsync_ShouldReturnCorrectly(int groupId, string senderId, string receiverId)
		{
			// Arrange
			var sender = new User { Id = senderId, SentGifts = new List<Gift>() };
			var receiver = new User { Id = receiverId, ReceivedGifts = new List<Gift>() };

			var mockedRepository = new Mock<IRepository<Gift>>();
			var mockedUnitOfWork = new Mock<IUnitOfWork>();

			var gift = new Gift();

			var mockedFactory = new Mock<IGiftFactory>();
			mockedFactory.Setup(f => f.CreateGift(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(gift);

			var mockedManager = new Mock<IGiftManager>();
			var mockedGroupService = new Mock<IGroupService>();

			var service = new GiftService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedManager.Object, mockedGroupService.Object);

			// Act
			var result = await service.CreateGiftAsync(groupId, sender, receiver);

			// Assert
			Assert.AreSame(gift, result);
		}
	}
}
