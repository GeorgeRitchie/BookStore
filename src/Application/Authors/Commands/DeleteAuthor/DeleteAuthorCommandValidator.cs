using Microsoft.EntityFrameworkCore;

namespace Application.Authors.Commands.DeleteAuthor
{
	public class DeleteAuthorCommandValidator : AbstractValidator<DeleteAuthorCommand>
	{
		public DeleteAuthorCommandValidator(IAppDb db)
		{
			RuleFor(i => i.Id).NotEmpty().CustomAsync(async (id, context, cancellationToken) =>
			{
				if (await db.Authors.GetAllAsNoTracking().AnyAsync(c => c.Id == id, cancellationToken) != true)
					context.AddFailure($"Author with id {id} not found.");
			});
		}
	}
}
