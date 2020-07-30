using System;

namespace SecretSanta.Application.Common.Exceptions
{
	public class EntityNotFoundException : Exception
	{
		public EntityNotFoundException(object key, Type type)
			: base($"Entity \"{type.Name}\" ({key}) was not found.")
		{ }
	}
}
