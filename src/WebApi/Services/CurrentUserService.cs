using Application.Common.Interfaces.Services;
using System.Security.Claims;

namespace WebApi.Services
{
	public class CurrentUserService : ICurrentUserService
	{
		private readonly IHttpContextAccessor httpContextAccessor;

		public CurrentUserService(IHttpContextAccessor httpContextAccessor)
		{
			this.httpContextAccessor = httpContextAccessor;
		}

		public Guid UserId => GetUserId(httpContextAccessor.HttpContext?.User?.Claims);

		public IEnumerable<Claim> UserClaims => httpContextAccessor.HttpContext?.User?.Claims ?? new List<Claim>();

		public Guid GetUserId(IEnumerable<Claim>? claims)
		{
			// these claimtypes should match to types in user claims generator of JWT token issuer
			var id = claims?.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value;
			return string.IsNullOrEmpty(id) ? Guid.Empty : Guid.Parse(id);
		}
	}
}
