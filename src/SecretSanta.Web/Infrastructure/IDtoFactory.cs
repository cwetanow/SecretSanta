using SecretSanta.Web.Models.Account;

namespace SecretSanta.Web.Infrastructure
{
    public interface IDtoFactory
    {
        TokenDto CreateTokenDto(string token);
    }
}
