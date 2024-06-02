using System.Security.Claims;

namespace Application.Common.Interfaces.Services
{
	public interface ICurrentUserService
	{
		Guid UserId { get; }
		IEnumerable<Claim> UserClaims { get; }
	}
}
