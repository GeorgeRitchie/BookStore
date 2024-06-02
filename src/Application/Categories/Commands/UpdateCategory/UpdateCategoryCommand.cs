using Application.Common.Models;

namespace Application.Categories.Commands.UpdateCategory
{
	// TODO enable when sign in part is done
	//[Authorize(Roles = $"{Role.SuperAdmin}")]
	public class UpdateCategoryCommand : IRequest<Result>
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public IFile? Icon { get; set; }
	}
}
