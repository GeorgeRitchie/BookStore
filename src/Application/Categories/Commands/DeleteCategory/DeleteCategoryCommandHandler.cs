using Application.Common.Enumerations;
using Application.Common.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Application.Categories.Commands.DeleteCategory
{
	public class DeleteCategoryCommandHandler(IAppDb db,
											  ILogger<DeleteCategoryCommandHandler> logger,
											  IFileManager fileManager)
										: IRequestHandler<DeleteCategoryCommand, Result>
	{
		public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
		{
			try
			{
				await db.BeginTransactionAsync(cancellationToken);

				var category = db.Categories.GetAll().FirstOrDefault(i => i.Id == request.Id)!;
				db.Categories.Delete(category);

				await db.SaveChangesAsync(cancellationToken);

				fileManager.Delete(category.Icon, ImageType.CategoryIcon);

				await db.CommitTransactionAsync(cancellationToken);

				return Result.Success();
			}
			catch (Exception ex)
			{
				logger.LogError("{source} - Something went wrong while removing category with id {id}. Ex: {@ex}",
												nameof(DeleteCategoryCommandHandler),
												request.Id, ex);

				await db.RollbackTransactionAsync(cancellationToken);

				throw;
			}
		}
	}
}
