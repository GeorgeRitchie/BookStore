using Microsoft.EntityFrameworkCore;

namespace Application.Books.Commands.UpdateBookAuthors
{
	public class UpdateBookAuthorsCommandValidator : AbstractValidator<UpdateBookAuthorsCommand>
	{
		public UpdateBookAuthorsCommandValidator(IAppDb db)
		{
			RuleFor(i => i.BookId).NotEmpty().CustomAsync(async (id, context, cancellationToken) =>
			{
				if (await db.Books.GetAllAsNoTracking().AnyAsync(c => c.Id == id, cancellationToken) != true)
					context.AddFailure($"Book with id {id} not found.");
			});

			When(i => i.AuthorIdToAdd is not null, () =>
			{
				RuleFor(i => i.AuthorIdToAdd).NotEmpty().CustomAsync(async (id, context, cancellationToken) =>
				{
					if (await db.Authors.GetAllAsNoTracking().AnyAsync(c => c.Id == id, cancellationToken) != true)
						context.AddFailure($"Author with id {id} not found.");
				});
			});

			When(i => i.AuthorIdToRemove is not null, () =>
			{
				RuleFor(i => i.AuthorIdToRemove).NotEmpty().CustomAsync(async (id, context, cancellationToken) =>
				{
					if (await db.Authors.GetAllAsNoTracking().AnyAsync(c => c.Id == id, cancellationToken) != true)
						context.AddFailure($"Author with id {id} not found.");
				});
			});
		}
	}
}
