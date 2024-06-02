namespace Application.Authors.Queries.GetAuthor
{
	public class GetAuthorQuery : IRequest<Author>
	{
		public Guid Id { get; set; }
	}
}
