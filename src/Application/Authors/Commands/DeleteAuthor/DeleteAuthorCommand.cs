namespace Application.Authors.Commands.DeleteAuthor
{
	// TODO enable when sign in part is done
	//[Authorize(Roles = $"{Role.SuperAdmin}")]
	public class DeleteAuthorCommand : IRequest<Result>
	{
		public Guid Id { get; set; }
	}
}
