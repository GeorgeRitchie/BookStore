using Application.Common.Models;

namespace Application.Books.Queries.GetBooks
{
	public class GetBooksQuery : IRequest<PaginatedList<BookDto>>
	{
		public Guid? FilterByCategoryId { get; set; }
		public Guid? FilterByAuthorId { get; set; }

		public PaginationParams? PaginationParams { get; set; }
	}
}
