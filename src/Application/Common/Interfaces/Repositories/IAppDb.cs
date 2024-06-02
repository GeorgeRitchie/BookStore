namespace Application.Common.Interfaces.Repositories
{
	public interface IAppDb : IDataBase
	{
		IRepository<User> Users { get; }
		IRepository<Author> Authors { get; }
		IRepository<Book> Books { get; }
		IRepository<Category> Categories { get; }
	}
}
