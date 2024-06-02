using Application.Common.Models;

namespace Application.Authors.Commands.UpdateAuthor
{
	// TODO enable when sign in part is done
	//[Authorize(Roles = $"{Role.SuperAdmin}")]
	public class UpdateAuthorCommand : IRequest<Result>
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public IFile? Photo { get; set; }
		public string? Description { get; set; }
	}
}
