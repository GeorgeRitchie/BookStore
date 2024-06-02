using Microsoft.EntityFrameworkCore;

namespace Application.Categories.Queries.GetCategory
{
	public class GetCategoryQueryHandler(IAppDb db) : IRequestHandler<GetCategoryQuery, Category>
	{
		public Task<Category> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
		{
			return db.Categories.GetAllAsNoTracking().FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken)!;
		}
	}
}
