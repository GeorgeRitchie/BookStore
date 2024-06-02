using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Books.Queries.GetBook
{
	public class GetBookQueryHandler(IAppDb db, IMapper mapper) : IRequestHandler<GetBookQuery, BookDto>
	{
		public Task<BookDto> Handle(GetBookQuery request, CancellationToken cancellationToken)
		{
			return db.Books.GetAllAsNoTracking()
									.Where(i => i.Id == request.Id)
									.ProjectTo<BookDto>(mapper.ConfigurationProvider)
									.FirstOrDefaultAsync(cancellationToken)!;
		}
	}
}
