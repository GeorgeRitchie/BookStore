using Application.Common.Interfaces.Services;

namespace Application.Authors.Commands.CreateAuthor
{
	public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
	{
		public CreateAuthorCommandValidator(IFileManager fileManager)
		{
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
