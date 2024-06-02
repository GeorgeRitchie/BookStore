using Application.Common.Enumerations;
using Application.Common.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Books.Commands.DeleteBook
{
	public class DeleteBookCommandHandler(IAppDb db,
											  ILogger<DeleteBookCommandHandler> logger,
											  IFileManager fileManager)
								: IRequestHandler<DeleteBookCommand, Result>
	{
		public async Task<Result> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
		{
			try
			{
				await db.BeginTransactionAsync(cancellationToken);

				var book = await db.Books.GetAll().FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
				db.Books.Delete(book);

				await db.SaveChangesAsync(cancellationToken);

				fileManager.Delete(book.Image, ImageType.BookImage);

				await db.CommitTransactionAsync(cancellationToken);

				return Result.Success();
			}
			catch (Exception ex)
			{
				logger.LogError("{source} - Something went wrong while removing book with id {id}. Ex: {@ex}",
												nameof(DeleteBookCommandHandler),
												request.Id, ex);

				await db.RollbackTransactionAsync(cancellationToken);

				throw;
			}
		}
	}
}
