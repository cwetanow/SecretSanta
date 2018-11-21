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
	public class DistributeGiftsTests
	{
		[TestCase("name", 2)]
		public async Task TestDistributeGifts_ShouldCallGroupServiceGetGroupUsers(string groupName, int groupId)
		{
			// Arrange
			var group = new Group { GroupName = groupName, Id = groupId };

			var mockedRepository = new Mock<IRepository<Gift>>();
			var mockedUnitOfWork = new Mock<IUnitOfWork>();
			var mockedFactory = new Mock<IGiftFactory>();
			var mockedManager = new Mock<IGiftManager>();
			var mockedGroupService = new Mock<IGroupService>();

			var service = new GiftService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedManager.Object, mockedGroupService.Object);

			// Act
			var result = await service.DistributeGifts(group);

			// Assert
			mockedGroupService.Verify(s => s.GetGroupUsers(groupName), Times.Once);
		}

		[TestCase("name", 2)]
		public async Task TestDistributeGifts_ShouldCallGiftManagerDistributeGifts(string groupName, int groupId)
		{
			// Arrange
			var group = new Group { GroupName = groupName, Id = groupId };

			var mockedRepository = new Mock<IRepository<Gift>>();
			var mockedUnitOfWork = new Mock<IUnitOfWork>();
			var mockedFactory = new Mock<IGiftFactory>();
			var mockedManager = new Mock<IGiftManager>();

			var groupUsers = new List<User> { new User(), new User() };
			var mockedGroupService = new Mock<IGroupService>();
			mockedGroupService.Setup(s => s.GetGroupUsers(It.IsAny<string>())).Returns(groupUsers);

			var service = new GiftService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedManager.Object, mockedGroupService.Object);

			// Act
			var result = await service.DistributeGifts(group);

			// Assert
			mockedManager.Verify(m => m.DistributeGifts(groupUsers, groupId), Times.Once);
		}

		[TestCase("name", 2)]
		public async Task TestDistributeGifts_ShouldCallRepositoryAddCorrectNumberOfTimes(string groupName, int groupId)
		{
			// Arrange
			var group = new Group { GroupName = groupName, Id = groupId };

			var mockedRepository = new Mock<IRepository<Gift>>();
			var mockedUnitOfWork = new Mock<IUnitOfWork>();
			var mockedFactory = new Mock<IGiftFactory>();

			var gifts = new List<Gift> { new Gift(), new Gift(), new Gift(), new Gift(), new Gift() };

			var mockedManager = new Mock<IGiftManager>();
			mockedManager.Setup(m => m.DistributeGifts(It.IsAny<IList<User>>(), It.IsAny<int>()))
				.Returns(gifts);

			var mockedGroupService = new Mock<IGroupService>();

			var service = new GiftService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedManager.Object, mockedGroupService.Object);

			// Act
			await service.DistributeGifts(group);

			// Assert
			mockedRepository.Verify(r => r.Add(It.IsAny<Gift>()), Times.Exactly(gifts.Count));
		}

		[TestCase("name", 2)]
		public async Task TestDistributeGifts_ShouldCallUnitOfWorkCommitAsync(string groupName, int groupId)
		{
			// Arrange
			var group = new Group { GroupName = groupName, Id = groupId };

			var mockedRepository = new Mock<IRepository<Gift>>();
			var mockedUnitOfWork = new Mock<IUnitOfWork>();
			var mockedFactory = new Mock<IGiftFactory>();

			var gifts = new List<Gift> { new Gift(), new Gift(), new Gift(), new Gift(), new Gift() };

			var mockedManager = new Mock<IGiftManager>();
			mockedManager.Setup(m => m.DistributeGifts(It.IsAny<IList<User>>(), It.IsAny<int>()))
				.Returns(gifts);

			var mockedGroupService = new Mock<IGroupService>();

			var service = new GiftService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedManager.Object, mockedGroupService.Object);

			// Act
			await service.DistributeGifts(group);

			// Assert
			mockedUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
		}

		[TestCase("name", 2)]
		public async Task TestDistributeGifts_ShouldReturnCorrectly(string groupName, int groupId)
		{
			// Arrange
			var group = new Group { GroupName = groupName, Id = groupId };

			var mockedRepository = new Mock<IRepository<Gift>>();
			var mockedUnitOfWork = new Mock<IUnitOfWork>();
			var mockedFactory = new Mock<IGiftFactory>();

			var gifts = new List<Gift> { new Gift(), new Gift(), new Gift(), new Gift(), new Gift() };

			var mockedManager = new Mock<IGiftManager>();
			mockedManager.Setup(m => m.DistributeGifts(It.IsAny<IList<User>>(), It.IsAny<int>()))
				.Returns(gifts);

			var mockedGroupService = new Mock<IGroupService>();

			var service = new GiftService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedManager.Object, mockedGroupService.Object);

			// Act
			var result = await service.DistributeGifts(group);

			// Assert
			CollectionAssert.AreEqual(gifts, result);
		}
	}
}
