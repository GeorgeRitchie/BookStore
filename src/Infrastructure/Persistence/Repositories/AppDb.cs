using Application.Common.Interfaces.Repositories;

namespace Infrastructure.Persistence.Repositories
{
	internal class AppDb(AppDbContext context) : DataBase<AppDbContext>(context), IAppDb
	{
		private IRepository<User>? users;
		public IRepository<User> Users => users ?? new Repository<User, AppDbContext>(_context);

		private IRepository<Book>? books;
		public IRepository<Book> Books => books ?? new Repository<Book, AppDbContext>(_context);

		private IRepository<Category>? categories;
		public IRepository<Category> Categories => categories ?? new Repository<Category, AppDbContext>(_context);

		private IRepository<Author>? authors;
		public IRepository<Author> Authors => authors ?? new Repository<Author, AppDbContext>(_context);
	}
}
