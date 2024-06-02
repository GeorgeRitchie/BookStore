using Application.Common.Models;

namespace Application.Categories.Commands.CreateCategory
{
	// TODO enable when sign in part is done
	//[Authorize(Roles = $"{Role.SuperAdmin}")]
	public class CreateCategoryCommand : IRequest<Result<Guid>>
	{
		public string Name { get; set; }
		public IFile? Icon { get; set; }
	}
}
