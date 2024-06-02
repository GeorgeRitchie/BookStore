using Microsoft.EntityFrameworkCore;

namespace Application.Categories.Queries.GetCategory
{
	public class GetCategoryQueryValidator : AbstractValidator<GetCategoryQuery>
	{
        public GetCategoryQueryValidator(IAppDb db)
		{
			RuleFor(i => i.Id).NotEmpty().CustomAsync(async (id, context, cancellationToken) =>
			{
				if (await db.Categories.GetAllAsNoTracking().AnyAsync(c => c.Id == id, cancellationToken) != true)
					context.AddFailure($"Category with id {id} not found.");
			});
		}
	}
}
