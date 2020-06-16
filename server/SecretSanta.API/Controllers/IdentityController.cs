using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Application.Users.Commands;

namespace SecretSanta.API.Controllers
{
	public class IdentityController : BaseController
	{
		public IdentityController(IMediator mediator) : base(mediator)
		{ }

		[HttpPost]
		[Route("api/users")]
		[AllowAnonymous]
		public async Task<int> RegisterUser([FromBody] RegisterUserCommand request) => await Mediator.Send(request);
	}
}
