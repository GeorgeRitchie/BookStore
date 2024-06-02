using Application.Common.Enumerations;
using Application.Common.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Application.Authors.Commands.CreateAuthor
{
	public class CreateAuthorCommandHandler(IAppDb db,
											ILogger<CreateAuthorCommandHandler> logger,
											IFileManager fileManager)
									: IRequestHandler<CreateAuthorCommand, Result<Guid>>
	{
		public async Task<Result<Guid>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
		{
			try
			{
				await db.BeginTransactionAsync(cancellationToken);

				string? relativeFilePath = null;

				if (request.Photo is not null)
				{
					relativeFilePath = fileManager.GetRelativePath(request.Photo, ImageType.AuthorPhoto);
					await fileManager.SaveAsync(request.Photo, ImageType.AuthorPhoto, cancellationToken);
				}

				var creationResult = Author.Create(request.FirstName,
													request.LastName,
													relativeFilePath,
													request.Description);

				if (creationResult.IsSuccess)
				{
					db.Authors.Create(creationResult.Value!);

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
				logger.LogError("{source} - Something went wrong while creating new author. Ex: {@ex}",
								nameof(CreateAuthorCommandHandler), ex);

				await db.RollbackTransactionAsync(cancellationToken);

				if (request.Photo is not null)
					fileManager.Delete(fileManager.GetAbsolutePath(request.Photo, ImageType.AuthorPhoto));

				throw;
			}
		}
	}
}
