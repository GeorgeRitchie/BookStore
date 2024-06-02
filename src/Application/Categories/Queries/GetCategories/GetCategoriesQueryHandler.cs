using Application.Common.Models;

namespace Application.Categories.Queries.GetCategories
{
	public class GetCategoriesQueryHandler(IAppDb db) : IRequestHandler<GetCategoriesQuery, PaginatedList<Category>>
	{
		public Task<PaginatedList<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
		{
			return Task.FromResult(
							PaginatedList<Category>.Create(db.Categories.GetAllAsNoTracking(),
															request.PaginationParams!.PageNumber,
															request.PaginationParams!.PageSize));
		}
	}
}
