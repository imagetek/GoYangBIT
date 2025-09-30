using System;
using System.IdentityModel.Tokens.Jwt;

namespace SSData.DashboardAPI
{
	public static class TokenService
	{
		public static string AccessToken = string.Empty;
		public static string RefreshToken = string.Empty;
		public static DateTime ValidUntil = DateTime.MinValue;
	}

	public record TokenInfo
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
	}

	public record UserLogin
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
