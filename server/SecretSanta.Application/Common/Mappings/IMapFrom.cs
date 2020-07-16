using AutoMapper;

namespace SecretSanta.Application.Common.Mappings
{
	public interface IMapFrom<T>
	{
		void ApplyMapping(Profile profile) => profile.CreateMap(typeof(T), this.GetType());
	}
}
