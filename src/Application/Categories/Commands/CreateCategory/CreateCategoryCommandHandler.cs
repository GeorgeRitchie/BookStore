using Application.Common.Enumerations;
using Application.Common.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Application.Categories.Commands.CreateCategory
{
	public class CreateCategoryCommandHandler(IAppDb db,
											  ILogger<CreateCategoryCommandHandler> logger,
											  IFileManager fileManager)
										: IRequestHandler<CreateCategoryCommand, Result<Guid>>
	{
		public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
		{
			try
			{
				await db.BeginTransactionAsync(cancellationToken);

				string? relativeFilePath = null;

				if (request.Icon is not null)
				{
					relativeFilePath = fileManager.GetRelativePath(request.Icon, ImageType.CategoryIcon);
					await fileManager.SaveAsync(request.Icon, ImageType.CategoryIcon, cancellationToken);
				}

				var creationResult = Category.Create(request.Name, relativeFilePath, db.Categories.GetAllAsNoTracking());

				if (creationResult.IsSuccess)
				{
					db.Categories.Create(creationResult.Value!);

					await db.SaveChangesAsync(cancellationToken);
					await db.CommitTransactionAsync(cancellationToken);

					return Result.Success(creationResult.Value!.Id);
				}
				else
				{
					await db.RollbackTransactionAsync(cancellationToken);

					return Result.Failure(Guid.Empty, creationResult.Errors);
				}
			}
			catch (Exception ex)
			{
				logger.LogError("{source} - Something went wrong while creating new category. Ex: {@ex}",
								nameof(CreateCategoryCommandHandler), ex);

				await db.RollbackTransactionAsync(cancellationToken);

				if (request.Icon is not null)
					fileManager.Delete(fileManager.GetAbsolutePath(request.Icon, ImageType.CategoryIcon));

				throw;
			}
		}
	}
}
