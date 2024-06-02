namespace Domain.Errors
{
	public static class DomainErrors
	{
		public static class Category
		{
			public static Error NullOrWhiteSpaceName()
				=> new("Domain.Category.Name.Empty", "Category's name cannot be null, empty, or whitespace.");

			public static Error NotUniqueName(string name)
				=> new("Domain.Category.Name.NotUnique", $"Category's name {name} is not unique.");
		}

		public static class Author
		{
			public static Error NullOrWhiteSpaceFirstName()
				=> new("Domain.Author.FirstName.Empty", "Author's first name cannot be null, empty, or whitespace.");

			public static Error NullOrWhiteSpaceLastName()
				=> new("Domain.Author.LastName.Empty", "Author's last name cannot be null, empty, or whitespace.");
		}

		public static class Book
		{
			public static Error NullOrWhiteSpaceTitle()
				=> new("Domain.Book.Title.Empty", "Book's title cannot be null, empty, or whitespace.");

			public static Error NegativePriceValue()
				=> new("Domain.Book.Price.NegativeValue", "Book's price cannot be less then 0.");

			public static Error NotUniqueISBN(string isbn)
				=> new("Domain.Book.ISBN.NotUnique", $"Book's ISBN {isbn} is not unique.");
		}
	}
}
