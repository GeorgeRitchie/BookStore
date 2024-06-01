namespace Application.Common.Behaviors
{
	internal sealed class UnhandledExceptionBehavior<TRequest, TResponse>
												: IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseRequest
	{
		public async Task<TResponse> Handle(TRequest request,
											  RequestHandlerDelegate<TResponse> next,
											  CancellationToken cancellationToken)
		{
			try
			{
				return await next();
			}
			catch (Exception ex)
			{
				var requestName = typeof(TRequest).Name;

				Log.Error(ex,
						  "Application Layer Request: Unhandled Exception for Request {Name} {@Request} {ExceptionName}",
						  requestName,
						  request,
						  ex.GetType().Name);

				throw;
			}
		}
	}
}
