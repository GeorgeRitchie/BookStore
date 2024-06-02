using Application.Common.Models;

namespace Application.Books.Commands.CreateBook
{
	// TODO enable when sign in part is done
	//[Authorize(Roles = $"{Role.SuperAdmin}")]
	public class CreateBookCommand : IRequest<Result<Guid>>
	{
		public string Title { get; set; }
		public IFile? Image { get; set; }
		public string? Description { get; set; }
		public string? ISBN { get; set; }
		public decimal Price { get; set; }
	}
}
