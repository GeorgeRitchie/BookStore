namespace Application.Books.Queries.GetBook
{
	public class GetBookQuery : IRequest<BookDto>
	{
		public Guid Id { get; set; }
	}
}
