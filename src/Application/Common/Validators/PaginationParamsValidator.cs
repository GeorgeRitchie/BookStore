using Application.Common.Models;

namespace Application.Common.Validators
{
	public class PaginationParamsValidator : AbstractValidator<PaginationParams>
	{
		public PaginationParamsValidator()
		{
			RuleFor(i => i.PageNumber).Custom((number, context) =>
			{
				if (number < 1)
					context.InstanceToValidate.PageNumber = 1;
				else
				{
					var temp = (number - 1) * context.InstanceToValidate.PageSize;
					if (temp >= int.MaxValue || temp < 0)
					{
						context.InstanceToValidate.PageNumber = 1;
						context.InstanceToValidate.PageSize = 10;
					}
				}
			});

			RuleFor(i => i.PageSize).Custom((size, context) =>
			{
				if (size < 1)
					context.InstanceToValidate.PageSize = 10;
				else
				{
					var temp = size * (context.InstanceToValidate.PageNumber - 1);
					if (temp >= int.MaxValue || temp < 0)
					{
						context.InstanceToValidate.PageNumber = 1;
						context.InstanceToValidate.PageSize = 10;
					}
				}
			});
		}
	}
}
