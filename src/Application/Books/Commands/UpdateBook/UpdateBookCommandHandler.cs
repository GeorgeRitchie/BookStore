using Application.Common.Enumerations;
using Application.Common.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Books.Commands.UpdateBook
{
	public class UpdateBookCommandHandler(IAppDb db,
											  ILogger<UpdateBookCommandHandler> logger,
											  IFileManager fileManager)
									: IRequestHandler<UpdateBookCommand, Result>
	{
		public async Task<Result> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
		{
			string? newFilePath = null;

			try
			{
				await db.BeginTransactionAsync(cancellationToken);
				var result = Result.Success();

				var book = await db.Books.GetAll().FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

				result = Result.Combine(book.UpdateTitle(request.Title),
										book.UpdateISBN(request.ISBN, db.Books.GetAllAsNoTracking()));

				if (request.Image is not null && result.IsSuccess)
				{
					if (book.Image != null)
						fileManager.Delete(book.Image, ImageType.BookImage);

					book.Image = fileManager.GetRelativePath(request.Image, ImageType.BookImage);
					newFilePath = await fileManager.SaveAsync(request.Image, ImageType.BookImage, cancellationToken);
				}
				else if (request.Image is null && book.Image is not null && result.IsSuccess)
				{
					fileManager.Delete(book.Image, ImageType.BookImage);
				}

				if (result.IsSuccess)
				{
					book.Description = request.Description;
					book.Price = request.Price;

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
				logger.LogError("{source} - Something went wrong while updating book with id {id}. Ex: {@ex}",
												nameof(UpdateBookCommandHandler),
												request.Id, ex);

				await db.RollbackTransactionAsync(cancellationToken);

				if (newFilePath is not null)
					fileManager.Delete(newFilePath);

				throw;
			}
		}
	}
}
