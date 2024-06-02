using Application.Common.Models;
using Application.Common.Validators;

namespace Application.Categories.Queries.GetCategories
{
	public class GetCategoriesQueryValidator : AbstractValidator<GetCategoriesQuery>
	{
		public GetCategoriesQueryValidator()
		{
			When(o => o.PaginationParams != null, () =>
			{
				RuleFor(o => o.PaginationParams).SetValidator(new PaginationParamsValidator()!);
			});

			When(o => o.PaginationParams == null, () =>
			{
				RuleFor(o => o.PaginationParams).Custom((p, context) =>
				{
					if (p == null)
						context.InstanceToValidate.PaginationParams = new PaginationParams();
				});
			});
		}
	}
}
