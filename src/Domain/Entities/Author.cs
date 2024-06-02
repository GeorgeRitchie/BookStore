namespace Domain.Entities
{
	public class Author : BaseClass
	{
		private List<Book> books = [];
		private string? photo;
		private string? description;

		public string FirstName { get; private set; }
		public string LastName { get; private set; }
		public string? Photo { get => photo; set => photo = value?.Trim(); }
		public string? Description { get => description; set => description = value?.Trim(); }

		public List<Book> Books { get => books; set => books = value ?? []; }

		// For EF Core Only!
		private Author()
		{
		}

		protected Author(string firstName,
						 string lastName,
						 string? photo = null,
						 string? description = null)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(firstName, nameof(firstName));
			ArgumentException.ThrowIfNullOrWhiteSpace(lastName, nameof(lastName));

			FirstName = firstName;
			LastName = lastName;
			Photo = photo;
			Description = description;
		}

		public static Result<Author> Create(string firstName,
						 string lastName,
						 string? photo = null,
						 string? description = null)
		{
			var result = Result.Success()
				.Ensure(() => string.IsNullOrWhiteSpace(firstName) == false, DomainErrors.Author.NullOrWhiteSpaceFirstName())
				.Ensure(() => string.IsNullOrWhiteSpace(lastName) == false, DomainErrors.Author.NullOrWhiteSpaceLastName());

			if (result.IsSuccess)
				return Result.Success(new Author(firstName, lastName, photo, description));
			else
				return Result.Failure<Author>(null!, result.Errors);
		}

		public Result UpdateFirstName(string firstName)
		{
			if (FirstName == firstName)
				return Result.Success();

			var result = Result.Success()
				.Ensure(() => string.IsNullOrWhiteSpace(firstName) == false, DomainErrors.Author.NullOrWhiteSpaceFirstName());

			if (result.IsSuccess)
				FirstName = firstName;

			return result;
		}

		public Result UpdateLastName(string lastName)
		{
			if (LastName == lastName)
				return Result.Success();

			var result = Result.Success()
				.Ensure(() => string.IsNullOrWhiteSpace(lastName) == false, DomainErrors.Author.NullOrWhiteSpaceLastName());

			if (result.IsSuccess)
				LastName = lastName;

			return result;
		}
	}
}
