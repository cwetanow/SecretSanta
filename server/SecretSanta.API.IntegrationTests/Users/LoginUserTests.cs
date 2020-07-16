using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using SecretSanta.API.IntegrationTests.Common;
using SecretSanta.API.IntegrationTests.Common.Extensions;
using SecretSanta.Application.Users.Commands;
using Xunit;

namespace SecretSanta.API.IntegrationTests.Users
{
	public class LoginUserTests : BaseTestFixture
	{
		public LoginUserTests(TestWebApplicationFactory factory) : base(factory)
		{ }

		[Theory]
		[InlineData("", "password")]
		[InlineData("username", "")]
		public async Task GivenInvalidParameters_ReturnsBadRequest(string username, string password)
		{
			// Arrange
			var body = new AuthenticateUserCommand() {
				Username = username,
				Password = password
			};

			var content = new StringContent(JsonConvert.SerializeObject(body));
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			// Act
			var response = await Client.PostAsync("api/login", content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		[Theory]
		[InlineData("username", "displayName", "email@email.com", "!QAZ2wsx")]
		public async Task GivenInvalidCredentials_ReturnsBadRequest(string username, string displayName, string email, string password)
		{
			// Arrange
			var registerCommand = new RegisterUserCommand {
				Username = username,
				DisplayName = displayName,
				Email = email,
				Password = password
			};

			await Client.PostJson<object>("api/users", registerCommand);

			var body = new AuthenticateUserCommand {
				Username = username,
				Password = password + "invalid"
			};

			var content = new StringContent(JsonConvert.SerializeObject(body));
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			// Act
			var response = await Client.PostAsync("api/login", content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		[Theory]
		[InlineData("username", "displayName", "email@email.com", "!QAZ2wsx")]
		public async Task GivenValidCredentials_ReturnsOk(string username, string displayName, string email, string password)
		{
			// Arrange
			var registerCommand = new RegisterUserCommand {
				Username = username,
				DisplayName = displayName,
				Email = email,
				Password = password
			};

			await Client.PostJson<object>("api/users", registerCommand);

			var body = new AuthenticateUserCommand {
				Username = username,
				Password = password 
			};

			var content = new StringContent(JsonConvert.SerializeObject(body));
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			// Act
			var response = await Client.PostAsync("api/login", content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
		}
	}
}
