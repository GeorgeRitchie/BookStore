namespace Domain.Entities
{
	public class Category : BaseClass
	{
		private List<Book> books = [];
		private string? icon;

		public string Name { get; private set; }
		public string? Icon { get => icon; set => icon = value?.Trim(); }

		public List<Book> Books { get => books; set => books = value ?? []; }

		// For EF Core Only!
		private Category()
		{
		}

		protected Category(string name, string? icon)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));

			Name = name;
			Icon = icon;
		}

		public static Result<Category> Create(string name, string? icon = null, IQueryable<Category>? categories = null)
		{
			var result = Result.Success()
					.Ensure(() => string.IsNullOrEmpty(name) == false, DomainErrors.Category.NullOrWhiteSpaceName())
					.Ensure(() => categories?.Any(c => c.Name == name) != true, DomainErrors.Category.NotUniqueName(name));

			if (result.IsSuccess)
				return Result.Success(new Category(name, icon));
			else
				return Result.Failure<Category>(null!, result.Errors);
		}

		public Result UpdateName(string name, IQueryable<Category>? categories = null)
		{
			if (Name == name)
				return Result.Success();

			var result = Result.Success()
				.Ensure(() => categories?.Any(c => c.Name == name) != true, DomainErrors.Category.NotUniqueName(name));

			if (result.IsSuccess)
				Name = name;

			return result;
		}
	}
}
