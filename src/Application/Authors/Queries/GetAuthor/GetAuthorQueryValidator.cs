using Microsoft.EntityFrameworkCore;

namespace Application.Authors.Queries.GetAuthor
{
	public class GetAuthorQueryValidator : AbstractValidator<GetAuthorQuery>
	{
		public GetAuthorQueryValidator(IAppDb db)
		{
			RuleFor(i => i.Id).NotEmpty().CustomAsync(async (id, context, cancellationToken) =>
			{
				if (await db.Authors.GetAllAsNoTracking().AnyAsync(c => c.Id == id, cancellationToken) != true)
					context.AddFailure($"Author with id {id} not found.");
			});
		}
	}
}
