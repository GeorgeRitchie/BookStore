using Application.Common.Interfaces.Services;

namespace Application.Categories.Commands.CreateCategory
{
	public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
	{
		public CreateCategoryCommandValidator(IFileManager fileManager)
		{
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
