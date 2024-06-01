namespace Application.Common.Interfaces.Repositories
{
	public interface IAppDb : IDataBase
	{
		IRepository<User> Users { get; }
	}
}
