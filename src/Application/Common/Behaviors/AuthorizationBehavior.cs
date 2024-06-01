using Application.Common.Exceptions;
using Application.Common.Interfaces.Services;
using System.Reflection;

namespace Application.Common.Behaviors
{
	// See https://github.com/jasontaylordev/CleanArchitecture

	internal sealed class AuthorizationBehavior<TRequest, TResponse>(ICurrentUserService currentUserService,
																	 IAppDb repository)
												: IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseRequest
	{
		public async Task<TResponse> Handle(TRequest request,
											  RequestHandlerDelegate<TResponse> next,
											  CancellationToken cancellationToken)
		{
			var authorizeAttributes = request.GetType().GetCustomAttribute<AuthorizeAttribute>(inherit: false);

			if (authorizeAttributes != null)
			{
				User? user = null;

				// Must be authenticated user
				if (currentUserService.UserId == Guid.Empty ||
					(user = repository.Users
										.GetAllAsNoTracking()
										.FirstOrDefault(i => i.Id == currentUserService.UserId)) == null)
				{
					throw new UnauthenticatedUserException();
				}

				// System rule for super admin every thing is allowed
				if (user.Role != Role.SuperAdmin)
				{
					// Permission-based authorization
					if (string.IsNullOrEmpty(authorizeAttributes.Roles) == false
							&& authorizeAttributes.Roles.Contains(user.Role) == false)
					{
						throw new AccessForbiddenException();
					}
				}
			}

			// User is authorized / authorization not required
			return await next();
		}
	}
}
