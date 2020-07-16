using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Application.Common.Exceptions;
using SecretSanta.Application.Common.Interfaces;
using SecretSanta.Application.Users.Responses;
using SecretSanta.Domain.Entities;

namespace SecretSanta.Application.Users.Commands
{
	public class AuthenticateUserCommand : IRequest<UserProfileResponse>
	{
		public string Username { get; set; }
		public string Password { get; set; }

		public class Handler : IRequestHandler<AuthenticateUserCommand, UserProfileResponse>
		{
			private readonly IIdentityService identityService;
			private readonly DbContext context;
			private readonly IMapper mapper;

			public Handler(IIdentityService identityService, DbContext context, IMapper mapper)
			{
				this.identityService = identityService;
				this.context = context;
				this.mapper = mapper;
			}

			public async Task<UserProfileResponse> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
			{
				var result = await identityService.AuthenticateUser(request.Username, request.Password);

				if (!result.Success)
				{
					throw new BadRequestException(result.Errors.ToArray());
				}

				var user = await context.Set<User>()
					.Where(u => u.Username == request.Username.Trim())
					.ProjectTo<UserProfileResponse>(mapper.ConfigurationProvider)
					.SingleOrDefaultAsync(cancellationToken);

				if (user is null)
				{
					throw new EntityNotFoundException<User>(request.Username);
				}

				return user;
			}
		}

		public class Validator : AbstractValidator<AuthenticateUserCommand>
		{
			public Validator()
			{
				RuleFor(c => c.Username).NotEmpty();
				RuleFor(c => c.Password).NotEmpty();
			}
		}
	}
}
