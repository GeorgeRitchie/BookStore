using Application.Common.Enumerations;
using Application.Common.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Authors.Commands.UpdateAuthor
{
	public class UpdateAuthorCommandHandler(IAppDb db,
											  ILogger<UpdateAuthorCommandHandler> logger,
											  IFileManager fileManager)
									: IRequestHandler<UpdateAuthorCommand, Result>
	{
		public async Task<Result> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
		{
			string? newFilePath = null;

			try
			{
				await db.BeginTransactionAsync(cancellationToken);
				var result = Result.Success();

				var author = await db.Authors.GetAll().FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

				result = Result.Combine(author.UpdateFirstName(request.FirstName),
										author.UpdateLastName(request.LastName));

				if (request.Photo is not null && result.IsSuccess)
				{
					if (author.Photo != null)
						fileManager.Delete(author.Photo, ImageType.AuthorPhoto);

					author.Photo = fileManager.GetRelativePath(request.Photo, ImageType.AuthorPhoto);
					newFilePath = await fileManager.SaveAsync(request.Photo, ImageType.AuthorPhoto, cancellationToken);
				}
				else if (request.Photo is null && author.Photo is not null && result.IsSuccess)
				{
					fileManager.Delete(author.Photo, ImageType.AuthorPhoto);
				}

				if (result.IsSuccess)
				{
					author.Description = request.Description;

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
				logger.LogError("{source} - Something went wrong while updating author with id {id}. Ex: {@ex}",
												nameof(UpdateAuthorCommandHandler),
												request.Id, ex);

				await db.RollbackTransactionAsync(cancellationToken);

				if (newFilePath is not null)
					fileManager.Delete(newFilePath);

				throw;
			}
		}
	}
}
