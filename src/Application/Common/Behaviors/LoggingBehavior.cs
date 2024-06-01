using Application.Common.Interfaces.Services;

namespace Application.Common.Behaviors
{
	internal sealed class LoggingBehavior<TRequest, TResponse>(ICurrentUserService currentUserService)
										: IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseRequest
	{
		public async Task<TResponse> Handle(TRequest request,
											  RequestHandlerDelegate<TResponse> next,
											  CancellationToken cancellationToken)
		{
			var requestName = typeof(TRequest).Name;

			var userId = currentUserService.UserId;

			Log.Debug("Application Layer Request: {Name} {@UserId} {@Request}",
				requestName, userId, request);

			return await next();
		}
	}
}
