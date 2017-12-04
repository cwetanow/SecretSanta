namespace SecretSanta.Web.Models.Account
{
    public class TokenDto
    {
        public TokenDto()
        {

        }

        public TokenDto(string token) : this()
        {
            this.Token = token;
        }

        public string Token { get; set; }
    }
}
