using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Application.Common.Helpers
{
	public class JwtTokenOptions
	{
		public string ISSUER { get; private set; }
		public string AUDIENCE { get; private set; }
		public string KEY { get; private set; }

		// TODO __##__ Change it if needed
		// To use same data in any point of app
		public static JwtTokenOptions Instantiate() =>
						new(Guid.NewGuid().ToString(), "BookStore.WebApi", "AnyClient");

		protected JwtTokenOptions(string? key, string issuer, string audience)
		{
			if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
				KEY = Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
			else
				KEY = key;

			if (string.IsNullOrEmpty(issuer))
			{
				throw new ArgumentNullException(nameof(issuer));
			}

			if (string.IsNullOrEmpty(audience))
			{
				throw new ArgumentNullException(nameof(audience));
			}

			ISSUER = issuer;
			AUDIENCE = audience;
		}

		public SymmetricSecurityKey GetSymmetricSecurityKey()
		{
			return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
		}

		public TokenValidationParameters GetTokenValidationParameters()
		{
			return new()
			{
				ClockSkew = TimeSpan.Zero,
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				RequireExpirationTime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = ISSUER,
				ValidAudience = AUDIENCE,
				IssuerSigningKey = GetSymmetricSecurityKey(),
			};
		}
	}
}
