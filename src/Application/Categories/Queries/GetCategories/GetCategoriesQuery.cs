using Application.Common.Models;

namespace Application.Categories.Queries.GetCategories
{
	public class GetCategoriesQuery : IRequest<PaginatedList<Category>>
	{
		public PaginationParams? PaginationParams { get; set; }
	}
}
