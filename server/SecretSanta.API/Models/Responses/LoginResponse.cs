﻿namespace SecretSanta.API.Models.Responses
{
	public class LoginResponse
	{
		public LoginResponse(string token)
		{
			Token = token;
		}

		public string Token { get; }
	}
}