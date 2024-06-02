using Application.Common.Enumerations;
using Application.Common.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Application.Books.Commands.CreateBook
{
	public class CreateBookCommandHandler(IAppDb db,
											  ILogger<CreateBookCommandHandler> logger,
											  IFileManager fileManager)
									: IRequestHandler<CreateBookCommand, Result<Guid>>
	{
		public async Task<Result<Guid>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
		{
			try
			{
				await db.BeginTransactionAsync(cancellationToken);

				string? relativeFilePath = null;

				if (request.Image is not null)
				{
					relativeFilePath = fileManager.GetRelativePath(request.Image, ImageType.BookImage);
					await fileManager.SaveAsync(request.Image, ImageType.BookImage, cancellationToken);
				}

				var creationResult = Book.Create(request.Title,
												 request.Price,
												 relativeFilePath,
												 request.Description,
												 request.ISBN,
												 books: db.Books.GetAllAsNoTracking());

				if (creationResult.IsSuccess)
				{
					db.Books.Create(creationResult.Value!);

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
				logger.LogError("{source} - Something went wrong while creating new book. Ex: {@ex}",
								nameof(CreateBookCommandHandler), ex);

				await db.RollbackTransactionAsync(cancellationToken);

				if (request.Image is not null)
					fileManager.Delete(fileManager.GetAbsolutePath(request.Image, ImageType.BookImage));

				throw;
			}
		}
	}
}
