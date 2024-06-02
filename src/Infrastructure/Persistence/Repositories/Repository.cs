using Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
	public class Repository<T, K>(K context) : IRepository<T> where T : class where K : DbContext
	{
		protected readonly DbSet<T> dbSet = context?.Set<T>() ?? throw new ArgumentNullException(nameof(context));

		public virtual T Create(T entity)
		{
			return dbSet.Add(entity).Entity;
		}

		public virtual void CreateRange(IEnumerable<T> entities)
		{
			dbSet.AddRange(entities);
		}

		public virtual void Update(T entity)
		{
			dbSet.Update(entity);
		}

		public virtual void UpdateRange(IEnumerable<T> entities)
		{
			dbSet.UpdateRange(entities);
		}

		public virtual void Delete(Guid id)
		{
			Delete(dbSet.Find(id)!);
		}

		public virtual void Delete(T entity)
		{
			dbSet.Remove(entity);
		}

		public virtual void DeleteRange(IEnumerable<T> entities)
		{
			dbSet.RemoveRange(entities);
		}

		public virtual IQueryable<T> GetAll()
		{
			return dbSet;
		}

		public virtual IQueryable<T> GetAllIgnoringQueryFilters()
		{
			return dbSet.IgnoreQueryFilters();
		}

		public virtual IQueryable<T> GetAllAsNoTracking()
		{
			return dbSet.AsNoTracking();
		}

		public virtual IQueryable<T> GetAllIgnoringQueryFiltersAsNoTracking()
		{
			return dbSet.IgnoreQueryFilters().AsNoTracking();
		}
	}
}
