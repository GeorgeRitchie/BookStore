namespace Application.Books.Commands.UpdateBookAuthors
{
	public class UpdateBookAuthorsCommand : IRequest<Result>
	{
		public Guid BookId { get; set; }
		public Guid? AuthorIdToRemove { get; set; }
		public Guid? AuthorIdToAdd { get; set; }
	}
}
