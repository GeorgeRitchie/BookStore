using Application.Common.Models;
using Application.Common.Validators;

namespace Application.Books.Queries.GetBooks
{
	public class GetBooksQueryValidator : AbstractValidator<GetBooksQuery>
	{
		public GetBooksQueryValidator()
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
