namespace Domain.Entities
{
	public class Book : BaseClass
	{
		private decimal price;
		private string? image;
		private string? description;
		private List<Category> categories = [];
		private List<Author> authors = [];

		public string Title { get; private set; }
		public string? Image { get => image; set => image = value?.Trim(); }
		public string? Description { get => description; set => description = value?.Trim(); }
		public string? ISBN { get; private set; }

		public decimal Price
		{
			get => price;
			set
			{
				if (value < 0)
					throw new ArgumentException("Price cannot be negative value.");

				price = value;
			}
		}

		public List<Category> Categories { get => categories; set => categories = value ?? []; }
		public List<Author> Authors { get => authors; set => authors = value ?? []; }

		// For EF Core Only!
		private Book()
		{
		}

		protected Book(string title,
					  decimal price,
					  string? image = null,
					  string? description = null,
					  string? iSBN = null,
					  List<Category>? categories = null,
					  List<Author>? authors = null)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(title, nameof(title));

			if (price < 0)
				throw new ArgumentException("Price cannot be negative value.");

			Title = title;
			Price = price;
			Image = image;
			Description = description;
			ISBN = iSBN?.Trim();
			Categories = categories ?? [];
			Authors = authors ?? [];
		}

		public static Result<Book> Create(string title,
										  decimal price,
										  string? image = null,
										  string? description = null,
										  string? iSBN = null,
										  List<Category>? categories = null,
										  List<Author>? authors = null,
										  IQueryable<Book>? books = null)
		{
			var result = Result.Success()
				.Ensure(() => string.IsNullOrWhiteSpace(title) == false, DomainErrors.Book.NullOrWhiteSpaceTitle())
				.Ensure(() => price >= 0, DomainErrors.Book.NegativePriceValue())
				.Ensure(() => iSBN == null || books?.Any(b => b.ISBN == iSBN.Trim()) != true,
							DomainErrors.Book.NotUniqueISBN(iSBN));

			if (result.IsSuccess)
				return Result.Success(new Book(title, price, image, description, iSBN, categories, authors));
			else
				return Result.Failure<Book>(null!, result.Errors);
		}

		public Result UpdateTitle(string title)
		{
			var result = Result.Success()
				.Ensure(() => string.IsNullOrWhiteSpace(title) == false, DomainErrors.Book.NullOrWhiteSpaceTitle());

			if (result.IsSuccess)
				Title = title;

			return result;
		}

		public Result UpdateISBN(string? isbn, IQueryable<Book>? books)
		{
			if (isbn is null || isbn.Trim() == string.Empty)
			{
				ISBN = isbn;
				return Result.Success();
			}

			var result = Result.Success()
				.Ensure(() => books?.Any(b => b.ISBN == isbn.Trim()) != true,
							DomainErrors.Book.NotUniqueISBN(isbn));

			if (result.IsSuccess)
				ISBN = isbn;

			return result;
		}
	}
}