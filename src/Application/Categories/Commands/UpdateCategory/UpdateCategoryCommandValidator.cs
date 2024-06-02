using Application.Common.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.Commands.UpdateCategory
{
	public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
	{
		public UpdateCategoryCommandValidator(IAppDb db, IFileManager fileManager)
		{
			RuleFor(i => i.Id).NotEmpty().CustomAsync(async (id, context, cancellationToken) =>
			{
				if (await db.Categories.GetAllAsNoTracking().AnyAsync(c => c.Id == id, cancellationToken) != true)
					context.AddFailure($"Category with id {id} not found.");
			});

			RuleFor(i => i.Name).NotEmpty();

			When(i => i.Icon is not null, () =>
			{
				RuleFor(i => i.Icon).Custom((icon, context) =>
				{
					if (fileManager.IsPhoto(icon) == false)
						context.AddFailure("Category icon must be a valid photo file.");

					if (icon.SizeInBytes > Constants.IconFileMaxSizeInBytes)
						context.AddFailure($"Category icon file size must be less or equal to {Constants.IconFileMaxSizeInBytes} bytes.");
				});
			});
		}
	}
}
