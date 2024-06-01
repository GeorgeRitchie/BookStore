using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Common.Helpers
{
	public static class JwtTokenProvider
	{
		public static string GenerateJwtTokenAsString(int tokenLifeTime_Min, JwtTokenOptions jwtTokenOptions, IEnumerable<Claim>? claims = null)
		{
			return ConvertJwtTokenToString(GenerateJwtToken(tokenLifeTime_Min, jwtTokenOptions, claims));
		}

		public static string ConvertJwtTokenToString(JwtSecurityToken token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			return tokenHandler.WriteToken(token);
		}

		public static JwtSecurityToken GenerateJwtToken(int tokenLifeTime_Min, JwtTokenOptions jwtTokenOptions, IEnumerable<Claim>? claims = null)
		{
			return new JwtSecurityToken(
				issuer: jwtTokenOptions.ISSUER,
				audience: jwtTokenOptions.AUDIENCE,
				claims: claims,
				notBefore: DateTime.UtcNow,
				expires: DateTime.UtcNow.AddMinutes(tokenLifeTime_Min),
				signingCredentials: new SigningCredentials(jwtTokenOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256Signature));
		}

		public static IEnumerable<Claim>? ExtractClaims(string token, JwtTokenOptions jwtTokenOptions)
		{
			try
			{
				JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

				var claimsPrincipal = jwtSecurityTokenHandler.ValidateToken(token, jwtTokenOptions.GetTokenValidationParameters(), out SecurityToken _);

				return claimsPrincipal.Claims;
			}
			catch
			{
				return null;
			}
		}
	}
}
