using Application.Common.Models;

namespace Application.Authors.Queries.GetAuthors
{
	public class GetAuthorsQueryHandler(IAppDb db) : IRequestHandler<GetAuthorsQuery, PaginatedList<Author>>
	{
		public Task<PaginatedList<Author>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
		{
			return Task.FromResult(
							PaginatedList<Author>.Create(db.Authors.GetAllAsNoTracking(),
															request.PaginationParams!.PageNumber,
															request.PaginationParams!.PageSize));
		}
	}
}
