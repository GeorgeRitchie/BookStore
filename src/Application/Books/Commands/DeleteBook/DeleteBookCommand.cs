namespace Application.Books.Commands.DeleteBook
{
	// TODO enable when sign in part is done
	//[Authorize(Roles = $"{Role.SuperAdmin}")]
	public class DeleteBookCommand : IRequest<Result>
	{
		public Guid Id { get; set; }
	}
}
