namespace Application.Books.Commands.UpdateBookCategories
{
	public class UpdateBookCategoriesCommand : IRequest<Result>
	{
		public Guid BookId { get; set; }
		public Guid? CategoryIdToRemove { get; set; }
		public Guid? CategoryIdToAdd { get; set; }
	}
}
