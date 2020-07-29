using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.API.Models.Responses;
using SecretSanta.Application.Users.Commands;
using SecretSanta.Identity.Interfaces;

namespace SecretSanta.API.Controllers
{
	public class IdentityController : BaseController
	{
		private readonly ITokenService tokenService;

		public IdentityController(IMediator mediator, ITokenService tokenService)
			: base(mediator)
		{
			this.tokenService = tokenService;
		}

		[HttpPost]
		[Route("api/users")]
		[AllowAnonymous]
		public async Task<int> RegisterUser([FromBody] RegisterUserCommand request) => await Mediator.Send(request);

		[HttpPost]
		[AllowAnonymous]
		[Route("api/login")]
		public async Task<LoginResponse> LoginUser([FromBody]AuthenticateUserCommand request)
		{
			var user = await Mediator.Send(request);

			var token = tokenService.EncodeToken(user.Username, user.Id, user.DisplayName);

			return new LoginResponse(token);
		}
	}
}
