using Application.Common.Enumerations;
using Application.Common.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Categories.Commands.UpdateCategory
{
	public class UpdateCategoryCommandHandler(IAppDb db,
											  ILogger<UpdateCategoryCommandHandler> logger,
											  IFileManager fileManager)
										: IRequestHandler<UpdateCategoryCommand, Result>
	{
		public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
		{
			string? newFilePath = null;

			try
			{
				await db.BeginTransactionAsync(cancellationToken);
				var result = Result.Success();

				var category = await db.Categories.GetAll().FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

				result = category.UpdateName(request.Name, db.Categories.GetAllAsNoTracking());

				if (request.Icon is not null && result.IsSuccess)
				{
					if (category.Icon != null)
						fileManager.Delete(category.Icon, ImageType.CategoryIcon);

					category.Icon = fileManager.GetRelativePath(request.Icon, ImageType.CategoryIcon);
					newFilePath = await fileManager.SaveAsync(request.Icon, ImageType.CategoryIcon, cancellationToken);
				}
				else if (request.Icon is null && category.Icon is not null && result.IsSuccess)
				{
					fileManager.Delete(category.Icon, ImageType.CategoryIcon);
				}

				if (result.IsSuccess)
				{
					await db.SaveChangesAsync(cancellationToken);
					await db.CommitTransactionAsync(cancellationToken);
				}
				else
				{
					await db.RollbackTransactionAsync(cancellationToken);
				}

				return result;
			}
			catch (Exception ex)
			{
				logger.LogError("{source} - Something went wrong while updating category with id {id}. Ex: {@ex}",
												nameof(UpdateCategoryCommandHandler),
												request.Id, ex);

				await db.RollbackTransactionAsync(cancellationToken);

				if (newFilePath is not null)
					fileManager.Delete(newFilePath);

				throw;
			}
		}
	}
}
