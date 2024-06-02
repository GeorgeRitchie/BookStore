using Microsoft.EntityFrameworkCore;

namespace Application.Books.Commands.UpdateBookCategories
{
	public class UpdateBookCategoriesCommandValidator : AbstractValidator<UpdateBookCategoriesCommand>
	{
		public UpdateBookCategoriesCommandValidator(IAppDb db)
		{
			RuleFor(i => i.BookId).NotEmpty().CustomAsync(async (id, context, cancellationToken) =>
			{
				if (await db.Books.GetAllAsNoTracking().AnyAsync(c => c.Id == id, cancellationToken) != true)
					context.AddFailure($"Book with id {id} not found.");
			});

			When(i => i.CategoryIdToAdd is not null, () =>
			{
				RuleFor(i => i.CategoryIdToAdd).NotEmpty().CustomAsync(async (id, context, cancellationToken) =>
				{
					if (await db.Categories.GetAllAsNoTracking().AnyAsync(c => c.Id == id, cancellationToken) != true)
						context.AddFailure($"Category with id {id} not found.");
				});
			});

			When(i => i.CategoryIdToRemove is not null, () =>
			{
				RuleFor(i => i.CategoryIdToRemove).NotEmpty().CustomAsync(async (id, context, cancellationToken) =>
				{
					if (await db.Categories.GetAllAsNoTracking().AnyAsync(c => c.Id == id, cancellationToken) != true)
						context.AddFailure($"Category with id {id} not found.");
				});
			});
		}
	}
}
