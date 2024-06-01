using Application.Common.Interfaces.Services;
using System.Diagnostics;

namespace Application.Common.Behaviors
{
	internal sealed class PerformanceBehavior<TRequest, TResponse>(ICurrentUserService currentUserService)
											: IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseRequest
	{
		private const long MaxAcceptableRequestHandledMilliseconds = 500;
		private readonly Stopwatch timer = new();

		public async Task<TResponse> Handle(TRequest request,
											  RequestHandlerDelegate<TResponse> next,
											  CancellationToken cancellationToken)
		{
			timer.Start();

			var response = await next();

			timer.Stop();

			var elapsedMilliseconds = timer.ElapsedMilliseconds;

			if (elapsedMilliseconds > MaxAcceptableRequestHandledMilliseconds)
			{
				var requestName = typeof(TRequest).Name;

				var userId = currentUserService.UserId;

				Log.Warning("Application Layer Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@Request}",
					requestName, elapsedMilliseconds, userId, request);
			}

			return response;
		}
	}
}
