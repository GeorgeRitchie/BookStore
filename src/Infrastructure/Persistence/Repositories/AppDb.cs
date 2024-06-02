using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories
{
	internal class AppDb(AppDbContext context) : DataBase<AppDbContext>(context), IAppDb
	{
		private IRepository<User>? users;
		public IRepository<User> Users => users ?? new Repository<User, AppDbContext>(_context);
	}
}
