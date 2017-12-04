namespace SecretSanta.Authentication.Contracts
{
    public interface ITokenManager
    {
        string GenerateToken(string email);
    }
}
