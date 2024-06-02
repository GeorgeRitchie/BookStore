using Application.Categories.Commands.DeleteCategory;
using Application.Common.Enumerations;
using Application.Common.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Authors.Commands.DeleteAuthor
{
	public class DeleteAuthorCommandHandler(IAppDb db,
											  ILogger<DeleteAuthorCommandHandler> logger,
											  IFileManager fileManager)
										: IRequestHandler<DeleteAuthorCommand, Result>
	{
		public async Task<Result> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
		{
			try
			{
				await db.BeginTransactionAsync(cancellationToken);

				var author = await db.Authors.GetAll().FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
				db.Authors.Delete(author);

				await db.SaveChangesAsync(cancellationToken);

				fileManager.Delete(author.Photo, ImageType.AuthorPhoto);

				await db.CommitTransactionAsync(cancellationToken);

				return Result.Success();
			}
			catch (Exception ex)
			{
				logger.LogError("{source} - Something went wrong while removing author with id {id}. Ex: {@ex}",
												nameof(DeleteAuthorCommandHandler),
												request.Id, ex);

				await db.RollbackTransactionAsync(cancellationToken);

				throw;
			}
		}
	}
}
