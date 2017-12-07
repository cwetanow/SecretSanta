using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Models;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta.Services.Tests.UserServiceTests
{
    [TestFixture]
    public class GetByUsernameTests
    {
        [TestCase("username")]
        [TestCase("myUsername")]
        public void TestGetByUsername_ShouldCallRepositoryAllCorrectly(string username)
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<User>>();

            var service = new UserService(mockedRepository.Object);

            // Act
            service.GetByUsername(username);

            // Assert
            mockedRepository.Verify(r => r.All, Times.Once());
        }

        [TestCase("username")]
        [TestCase("myUsername")]
        public void TestGetByUsername_ShouldReturnCorrectly(string username)
        {
            // Arrange
            var user = new User { UserName = username };

            var mockedRepository = new Mock<IRepository<User>>();
            mockedRepository.Setup(r => r.All)
                .Returns(new List<User> { user }.AsQueryable());

            var service = new UserService(mockedRepository.Object);

            // Act
            var result = service.GetByUsername(username);

            // Assert
            Assert.AreSame(user, result);
        }

        [TestCase("username")]
        [TestCase("myUsername")]
        public void TestGetByUsername_NoUser_ShouldReturnNull(string username)
        {
            // Arrange
            var user = new User { UserName = string.Empty };

            var mockedRepository = new Mock<IRepository<User>>();
            mockedRepository.Setup(r => r.All)
                .Returns(new List<User> { user }.AsQueryable());

            var service = new UserService(mockedRepository.Object);

            // Act
            var result = service.GetByUsername(username);

            // Assert
            Assert.IsNull(result);
        }
    }
}
