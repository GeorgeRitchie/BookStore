using Microsoft.EntityFrameworkCore;

namespace Application.Categories.Commands.DeleteCategory
{
	public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
	{
		public DeleteCategoryCommandValidator(IAppDb db)
		{
			RuleFor(i => i.Id).NotEmpty().CustomAsync(async (id, context, cancellationToken) =>
			{
				if (await db.Categories.GetAllAsNoTracking().AnyAsync(c => c.Id == id, cancellationToken) != true)
					context.AddFailure($"Category with id {id} not found.");
			});
		}
	}
}
