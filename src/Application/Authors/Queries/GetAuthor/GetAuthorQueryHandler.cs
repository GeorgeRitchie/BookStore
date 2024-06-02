using Microsoft.EntityFrameworkCore;

namespace Application.Authors.Queries.GetAuthor
{
	public class GetAuthorQueryHandler(IAppDb db) : IRequestHandler<GetAuthorQuery, Author>
	{
		public Task<Author> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
		{
			return db.Authors.GetAllAsNoTracking().FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken)!;
		}
	}
}
