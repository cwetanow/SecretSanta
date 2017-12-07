namespace SecretSanta.Common
{
    public static class Constants
    {
        public const string UserAlreadyExists = "User already exists";
        public const string InvalidCredentials = "Invalid Credentials";

        public const string TokenKey = "Tokens:Key";
        public const string TokenIssuer = "Tokens:Issuer";

        public const int TokenExpireHours = 2;
        public const int TokenExpireMinutes = 0;
        public const int TokenExpireSeconds = 0;

        public const string UsernameCannotBeNull = "Username cannot be null";
        public const string UserNotFound = "User not found";
    }
}
