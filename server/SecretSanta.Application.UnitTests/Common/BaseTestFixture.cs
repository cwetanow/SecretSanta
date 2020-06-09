﻿using System;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Data;

namespace SecretSanta.Application.UnitTests.Common
{
	public class BaseTestFixture : IDisposable
	{
		protected BaseTestFixture()
		{
			Context = CreateContext();
		}

		protected SecretSantaContext Context { get; }

		private SecretSantaContext CreateContext()
		{
			var options = new DbContextOptionsBuilder<SecretSantaContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
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
