namespace SecretSanta.Identity.Interfaces
{
	public interface ITokenService
	{
		string EncodeToken(string username, int userId, string displayName);
	}
}
