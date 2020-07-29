using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Application.Users.Responses;
using SecretSanta.Domain.Entities;

namespace SecretSanta.Application.Users.Queries
{
	public class UserListQuery : IRequest<UserListResponse>
	{
		public string SearchPattern { get; set; }

		public bool SortAscending { get; set; } = true;

		public int Limit { get; set; } = 10;
		public int Offset { get; set; } = 0;

		public class Handler : IRequestHandler<UserListQuery, UserListResponse>
		{
			private readonly DbContext context;
			private readonly IMapper mapper;

			public Handler(DbContext context, IMapper mapper)
			{
				this.context = context;
				this.mapper = mapper;
			}

			public async Task<UserListResponse> Handle(UserListQuery request, CancellationToken cancellationToken)
			{
				IQueryable<User> query = context.Set<User>();

				if (!string.IsNullOrEmpty(request.SearchPattern))
				{
					query = query
						.Where(u => u.DisplayName.Contains(request.SearchPattern) || u.Username.Contains(request.SearchPattern));
				}

				if (request.SortAscending)
				{
					query = query
						.OrderBy(u => u.DisplayName);
				}
				else
				{
					query = query
						.OrderByDescending(u => u.DisplayName);
				}

				var users = await query
					.Skip(request.Offset)
					.Take(request.Limit)
					.ProjectTo<UserProfileResponse>(mapper.ConfigurationProvider)
					.ToListAsync(cancellationToken);

				return new UserListResponse {
					Users = users
				};
			}
		}
	}
}
