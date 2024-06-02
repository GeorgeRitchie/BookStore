namespace Application.Categories.Commands.DeleteCategory
{
	// TODO enable when sign in part is done
	//[Authorize(Roles = $"{Role.SuperAdmin}")]
	public class DeleteCategoryCommand : IRequest<Result>
	{
		public Guid Id { get; set; }
	}
}
