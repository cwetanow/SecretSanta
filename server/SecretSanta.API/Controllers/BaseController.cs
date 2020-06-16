using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SecretSanta.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public abstract class BaseController : ControllerBase
	{
		protected readonly IMediator Mediator;

		protected BaseController(IMediator mediator)
		{
			this.Mediator = mediator;
		}
	}
}
