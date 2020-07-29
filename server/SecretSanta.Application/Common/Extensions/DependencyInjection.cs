using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SecretSanta.Application.Common.Behaviours;

namespace SecretSanta.Application.Common.Extensions
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services
				.AddMediatR(Assembly.GetExecutingAssembly())
				.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

			services
				.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

			services
				.AddAutoMapper(Assembly.GetExecutingAssembly());

			return services;
		}
	}
}
