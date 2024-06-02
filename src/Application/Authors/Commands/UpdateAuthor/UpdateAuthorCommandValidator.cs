using Application.Common.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Application.Authors.Commands.UpdateAuthor
{
	public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
	{
		public UpdateAuthorCommandValidator(IAppDb db, IFileManager fileManager)
		{
			RuleFor(i => i.Id).NotEmpty().CustomAsync(async (id, context, cancellationToken) =>
			{
				if (await db.Authors.GetAllAsNoTracking().AnyAsync(c => c.Id == id, cancellationToken) != true)
					context.AddFailure($"Author with id {id} not found.");
			});

			RuleFor(i => i.FirstName).NotEmpty();
			RuleFor(i => i.LastName).NotEmpty();

			When(i => i.Photo is not null, () =>
			{
				RuleFor(i => i.Photo).Custom((photo, context) =>
				{
					if (fileManager.IsPhoto(photo) == false)
						context.AddFailure("Author's photo must be a valid photo file.");

					if (photo.SizeInBytes > Constants.AuthorPhotoFileMaxSizeInBytes)
						context.AddFailure($"Author's photo file size must be less or equal to {Constants.AuthorPhotoFileMaxSizeInBytes} bytes.");
				});
			});
		}
	}
}
