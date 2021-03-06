﻿using Moq;
using NUnit.Framework;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using SecretSanta.Models;
using SecretSanta.Providers.Contracts;
using SecretSanta.Services.Contracts;

namespace SecretSanta.Services.Tests.InviteServiceTests
{
    [TestFixture]
    public class ConstructorTests
    {
        [Test]
        public void TestConstructor_ShouldInitializeCorrectly()
        {
            // Arrange
            var mockedRepository = new Mock<IRepository<Invite>>();
            var mockedUnitOfWork = new Mock<IUnitOfWork>();
            var mockedFactory = new Mock<IInviteFactory>();
            var mockedDateTimeProvider = new Mock<IDateTimeProvider>();

            // Act
            var service = new InviteService(mockedRepository.Object, mockedUnitOfWork.Object, mockedFactory.Object, mockedDateTimeProvider.Object);

            // Assert
            Assert.IsNotNull(service);
            Assert.IsInstanceOf<IInviteService>(service);
        }
    }
}
