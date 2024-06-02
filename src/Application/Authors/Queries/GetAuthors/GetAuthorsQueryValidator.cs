using Application.Common.Models;
using Application.Common.Validators;

namespace Application.Authors.Queries.GetAuthors
{
	public class GetAuthorsQueryValidator : AbstractValidator<GetAuthorsQuery>
	{
		public GetAuthorsQueryValidator()
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
