using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Books.Commands.UpdateBookAuthors
{
	public class UpdateBookAuthorsCommandHandler(IAppDb db,
													ILogger<UpdateBookAuthorsCommandHandler> logger)
												: IRequestHandler<UpdateBookAuthorsCommand, Result>
	{
		public async Task<Result> Handle(UpdateBookAuthorsCommand request, CancellationToken cancellationToken)
		{
			if (request.AuthorIdToAdd == null && request.AuthorIdToRemove == null)
				return Result.Success();

			try
			{
				await db.BeginTransactionAsync(cancellationToken);

				var book = await db.Books.GetAll()
											.Include(b => b.Authors)
											.FirstOrDefaultAsync(i => i.Id == request.BookId, cancellationToken);

				if (request.AuthorIdToRemove != null)
				{
					book.Authors.RemoveAll(i => i.Id == request.AuthorIdToRemove);
				}

				if (request.AuthorIdToAdd != null)
				{
					var newAuthor = await db.Authors.GetAll()
												.FirstOrDefaultAsync(c => c.Id == request.AuthorIdToAdd, cancellationToken);

					book.Authors.Add(newAuthor);
				}

				await db.SaveChangesAsync(cancellationToken);
				await db.CommitTransactionAsync(cancellationToken);

				return Result.Success();
			}
			catch (Exception ex)
			{
				logger.LogError("{source} - Something went wrong while updating book->author relation. Book with id {id}. Ex: {@ex}",
												nameof(UpdateBookAuthorsCommandHandler),
												request.BookId, ex);

				await db.RollbackTransactionAsync(cancellationToken);

				throw;
			}
		}
	}
}
