using Application.Common.Interfaces.Services;

namespace Application.Books.Commands.CreateBook
{
	public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
	{
		public CreateBookCommandValidator(IFileManager fileManager)
		{
			RuleFor(i => i.Title).NotEmpty();

			RuleFor(i => i.Price).GreaterThanOrEqualTo(0);

			When(i => i.Image is not null, () =>
			{
				RuleFor(i => i.Image).Custom((image, context) =>
				{
					if (fileManager.IsPhoto(image) == false)
						context.AddFailure("Book image must be a valid photo file.");

					if (image.SizeInBytes > Constants.BookImageFileMaxSizeInBytes)
						context.AddFailure($"Book image file size must be less or equal to {Constants.BookImageFileMaxSizeInBytes} bytes.");
				});
			});
		}
	}
}
