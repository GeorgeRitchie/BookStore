namespace Application.Categories.Queries.GetCategory
{
	public class GetCategoryQuery : IRequest<Category>
	{
		public Guid Id { get; set; }
	}
}
