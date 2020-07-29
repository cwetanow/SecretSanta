using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Application.Common.Mappings;
using SecretSanta.Persistence;

namespace SecretSanta.Application.UnitTests.Common
{
	public abstract class BaseTestFixture : IDisposable
	{
		protected BaseTestFixture()
		{
			Context = CreateContext();

			var configurationProvider = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
			Mapper = configurationProvider.CreateMapper();
		}

		protected SecretSantaContext Context { get; }
		protected IMapper Mapper { get; }

		private SecretSantaContext CreateContext()
		{
			var options = new DbContextOptionsBuilder<SecretSantaContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.EnableSensitiveDataLogging()
				.Options;

			var context = new SecretSantaContext(options);
			context.Database.EnsureCreated();

			return context;
		}

		public void Dispose()
		{
			Context.Database.EnsureDeleted();
			Context.Dispose();
		}
	}
}
