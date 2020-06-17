using System.Net.Http;
using Xunit;

namespace SecretSanta.API.IntegrationTests.Common
{
	public abstract class BaseTestFixture : IClassFixture<TestWebApplicationFactory>
	{
		protected readonly TestWebApplicationFactory Factory;
		protected readonly HttpClient Client;

		protected BaseTestFixture(TestWebApplicationFactory factory)
		{
			factory.CleanupDatabase();

			Factory = factory;
			Client = factory.CreateClient();
		}
	}
}
