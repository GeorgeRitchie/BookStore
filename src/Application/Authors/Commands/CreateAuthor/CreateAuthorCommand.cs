using Application.Common.Models;

namespace Application.Authors.Commands.CreateAuthor
{
	// TODO enable when sign in part is done
	//[Authorize(Roles = $"{Role.SuperAdmin}")]
	public class CreateAuthorCommand : IRequest<Result<Guid>>
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public IFile? Photo { get; set; }
		public string? Description { get; set; }
	}
}
