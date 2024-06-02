using Application.Common.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Application.Books.Commands.UpdateBook
{
	public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
	{
		public UpdateBookCommandValidator(IAppDb db, IFileManager fileManager)
		{
			RuleFor(i => i.Id).NotEmpty().CustomAsync(async (id, context, cancellationToken) =>
			{
				if (await db.Books.GetAllAsNoTracking().AnyAsync(c => c.Id == id, cancellationToken) != true)
					context.AddFailure($"Book with id {id} not found.");
			});

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
