namespace Application.Common.Interfaces.Repositories
{
	public interface IRepository<TEntity> where TEntity : class
	{
		TEntity Create(TEntity entity);
		void CreateRange(IEnumerable<TEntity> entities);

		void Update(TEntity entity);
		void UpdateRange(IEnumerable<TEntity> entities);

		void Delete(Guid id);
		void Delete(TEntity entity);
		void DeleteRange(IEnumerable<TEntity> entities);

		IQueryable<TEntity> GetAll();
		IQueryable<TEntity> GetAllAsNoTracking();
		IQueryable<TEntity> GetAllIgnoringQueryFilters();
		IQueryable<TEntity> GetAllIgnoringQueryFiltersAsNoTracking();
	}
}
