﻿using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace SecretSanta.Application.Common.Mappings
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
		}

		private void ApplyMappingsFromAssembly(Assembly assembly)
		{
			var types = assembly.GetExportedTypes()
				.Where(t => t.GetInterfaces().Any(i =>
					i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
				.ToList();

			foreach (var type in types)
			{
				var instance = Activator.CreateInstance(type);
				var methodInfo = type.GetMethod("ApplyMapping");

				if (methodInfo == null)
				{
					var genericParameters = type.GetInterfaces()
						.Where(i => i.Name == typeof(IMapFrom<>).Name)
						.SelectMany(i => i.GetGenericArguments())
						.ToList();

					foreach (var genericParameter in genericParameters)
					{
						CreateMap(genericParameter, type);
					}
				}
				else
				{
					methodInfo.Invoke(instance, new object[] { this });
				}
			}
		}
	}
}
