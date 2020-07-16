namespace SecretSanta.Identity.Interfaces
{
	public interface ITokenService
	{
		string EncodeToken(string username, int applicationUserId, string displayName);
	}
}
