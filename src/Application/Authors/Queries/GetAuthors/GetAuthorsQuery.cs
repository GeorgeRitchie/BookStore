using Application.Common.Models;

namespace Application.Authors.Queries.GetAuthors
{
	public class GetAuthorsQuery : IRequest<PaginatedList<Author>>
	{
		public PaginationParams? PaginationParams { get; set; }
	}
}
