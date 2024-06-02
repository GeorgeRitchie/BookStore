using Microsoft.EntityFrameworkCore;

namespace Application.Books.Queries.GetBook
{
	public class GetBookQueryValidator : AbstractValidator<GetBookQuery>
	{
		public GetBookQueryValidator(IAppDb db)
		{
			RuleFor(i => i.Id).NotEmpty().CustomAsync(async (id, context, cancellationToken) =>
			{
				if (await db.Books.GetAllAsNoTracking().AnyAsync(c => c.Id == id, cancellationToken) != true)
					context.AddFailure($"Book with id {id} not found.");
			});
		}
	}
}
