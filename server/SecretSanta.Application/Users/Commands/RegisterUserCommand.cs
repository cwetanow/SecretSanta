using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Application.Common.Exceptions;
using SecretSanta.Application.Common.Interfaces;
using SecretSanta.Domain.Entities;

namespace SecretSanta.Application.Users.Commands
{
	public class RegisterUserCommand : IRequest<int>
	{
		public string Email { get; set; }

		public string Username { get; set; }

		public string DisplayName { get; set; }

		public string Password { get; set; }

		public class Handler : IRequestHandler<RegisterUserCommand, int>
		{
			private readonly IUserService userService;
			private readonly DbContext context;

			public Handler(IUserService userService, DbContext context)
			{
				this.userService = userService;
				this.context = context;
			}

			public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
			{
				var (result, userId) = await userService.CreateUser(request.Username, request.Email, request.Password);

				if (!result.Success)
				{
					throw new BadRequestException(result.Errors.ToArray());
				}

				var user = new User(userId, request.Username, request.Email, request.DisplayName);
				context.Add(user);

				await context.SaveChangesAsync(cancellationToken);

				return user.Id;
			}
		}

		public class Validator : AbstractValidator<RegisterUserCommand>
		{
			public Validator()
			{
				RuleFor(c => c.Email).NotEmpty().EmailAddress(EmailValidationMode.Net4xRegex);
				RuleFor(c => c.Username).NotEmpty();
				RuleFor(c => c.Password).NotEmpty();
				RuleFor(c => c.DisplayName).NotEmpty();
			}
		}
	}
}
