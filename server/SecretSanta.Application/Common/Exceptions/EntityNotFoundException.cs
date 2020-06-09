using System;
using SecretSanta.Domain.Common;

namespace SecretSanta.Application.Common.Exceptions
{
	public class EntityNotFoundException<TEntity> : Exception
		where TEntity : Entity
	{
		public EntityNotFoundException(object key)
			: base($"Entity \"{typeof(TEntity).Name}\" ({key}) was not found.")
		{ }
	}
}
