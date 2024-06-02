using Application.Common.Models;
using AutoMapper.QueryableExtensions;

namespace Application.Books.Queries.GetBooks
{
	public class GetBooksQueryHandler(IAppDb db, IMapper mapper) : IRequestHandler<GetBooksQuery, PaginatedList<BookDto>>
	{
		public Task<PaginatedList<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
		{
			var books = db.Books.GetAllAsNoTracking();

			books = AddFilter(request, books);

			return Task.FromResult(
							PaginatedList<BookDto>.Create(books.ProjectTo<BookDto>(mapper.ConfigurationProvider),
															request.PaginationParams!.PageNumber,
															request.PaginationParams!.PageSize));
		}

		private IQueryable<Book> AddFilter(GetBooksQuery request, IQueryable<Book> query)
		{
			if (request.FilterByAuthorId is not null)
				query = query.Where(b => b.Authors.Any(a => a.Id == request.FilterByAuthorId));

			if (request.FilterByCategoryId is not null)
				query = query.Where(b => b.Categories.Any(c => c.Id == request.FilterByCategoryId));

			return query;
		}
	}
}
