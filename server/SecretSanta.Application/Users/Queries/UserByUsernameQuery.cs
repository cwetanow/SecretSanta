using FluentValidation;
using MediatR;
using SecretSanta.Application.Users.Responses;

namespace SecretSanta.Application.Users.Queries
{
	public class UserByUsernameQuery : IRequest<UserProfileResponse>
	{
		public string Username { get; set; }

		public class Validator : AbstractValidator<UserByUsernameQuery>
		{
			public Validator()
			{
				RuleFor(q => q.Username).NotEmpty();
			}
		}
	}
}
