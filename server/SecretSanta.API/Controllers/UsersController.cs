using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Application.Users.Queries;
using SecretSanta.Application.Users.Responses;

namespace SecretSanta.API.Controllers
{
	[Route("api/[controller]")]
	public class UsersController : BaseController
	{
		public UsersController(IMediator mediator) : base(mediator)
		{ }

		[HttpGet]
		public Task<UserListResponse> GetUsers([FromQuery] UserListQuery query) => Mediator.Send(query);

		[HttpGet("{username}")]
		public Task<UserProfileResponse> GetUserByUsername(string username) =>
			Mediator.Send(new UserByUsernameQuery { Username = username });
	}
}
