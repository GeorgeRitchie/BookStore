using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Books.Commands.UpdateBookCategories
{
	public class UpdateBookCategoriesCommandHandler(IAppDb db,
													ILogger<UpdateBookCategoriesCommandHandler> logger)
												: IRequestHandler<UpdateBookCategoriesCommand, Result>
	{
		public async Task<Result> Handle(UpdateBookCategoriesCommand request, CancellationToken cancellationToken)
		{
			if (request.CategoryIdToAdd == null && request.CategoryIdToRemove == null)
				return Result.Success();

			try
			{
				await db.BeginTransactionAsync(cancellationToken);

				var book = await db.Books.GetAll()
											.Include(b => b.Categories)
											.FirstOrDefaultAsync(i => i.Id == request.BookId, cancellationToken);

				if (request.CategoryIdToRemove != null)
				{
					book.Categories.RemoveAll(i => i.Id == request.CategoryIdToRemove);
				}

				if (request.CategoryIdToAdd != null)
				{
					var newCategory = await db.Categories.GetAll()
												.FirstOrDefaultAsync(c => c.Id == request.CategoryIdToAdd, cancellationToken);

					book.Categories.Add(newCategory);
				}

				await db.SaveChangesAsync(cancellationToken);
				await db.CommitTransactionAsync(cancellationToken);

				return Result.Success();
			}
			catch (Exception ex)
			{
				logger.LogError("{source} - Something went wrong while updating book->category relation. Book with id {id}. Ex: {@ex}",
												nameof(UpdateBookCategoriesCommandHandler),
												request.BookId, ex);

				await db.RollbackTransactionAsync(cancellationToken);

				throw;
			}
		}
	}
}
