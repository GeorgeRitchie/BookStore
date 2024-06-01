using Application.Common.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Common.Extensions
{
	public static class AuthenticationByJwtTokenExtensions
	{
		public static IServiceCollection AddAuthenticationByJwtToken(this IServiceCollection services)
		{
			// Adding Authentication
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			 {
				 options.RequireHttpsMetadata = false;
				 options.SaveToken = true;
				 options.TokenValidationParameters = JwtTokenOptions.Instantiate().GetTokenValidationParameters();
			 });

			return services;
		}
	}
}
