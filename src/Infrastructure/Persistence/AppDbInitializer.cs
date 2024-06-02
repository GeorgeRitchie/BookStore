using Application.Common.Interfaces.Repositories;
using AutoMapper;

namespace Infrastructure.Persistence
{
	public static class AppDbInitializer
	{
		private static IMapper _mapper;
		private static IAppDb _db;

		public static void Initialize(IAppDb db, IMapper mapper)
		{
			_mapper = mapper;
			_db = db;
			try
			{
				db.BeginTransaction();

				// TODO add code to automatically add a SuperAdmin if there is no user with such role in db.

				// Create seeder method for each entity as static method and call them here

				db.SaveChanges();
				db.CommitTransaction();
			}
			catch (Exception)
			{
				db.RollbackTransaction();
				throw;
			}
		}
	}
}
