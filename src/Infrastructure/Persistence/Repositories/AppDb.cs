using Application.Common.Interfaces.Repositories;

namespace Infrastructure.Persistence.Repositories
{
	internal class AppDb(AppDbContext context) : DataBase<AppDbContext>(context), IAppDb
	{
	}
}
