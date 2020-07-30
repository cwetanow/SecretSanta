using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Application.Common.Exceptions;
using SecretSanta.Application.Users.Responses;
using SecretSanta.Domain.Entities;

namespace SecretSanta.Application.Users.Queries
{
	public class UserByUsernameQuery : IRequest<UserProfileResponse>
	{
		public string Username { get; set; }

		public class Handler : IRequestHandler<UserByUsernameQuery, UserProfileResponse>
		{
			private readonly DbContext context;
			private readonly IMapper mapper;

			public Handler(DbContext context, IMapper mapper)
			{
				this.context = context;
				this.mapper = mapper;
			}

			public async Task<UserProfileResponse> Handle(UserByUsernameQuery request, CancellationToken cancellationToken)
			{
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

		public class Validator : AbstractValidator<UserByUsernameQuery>
		{
			public Validator()
			{
				RuleFor(q => q.Username).NotEmpty();
			}
		}
	}
}
