using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces.Repositories;
using System.Data;

namespace Infrastructure.Persistence.Repositories
{
	public abstract class DataBase<T>(T context) : IDataBase where T : DbContext
	{
		protected readonly T _context = context ?? throw new ArgumentNullException(nameof(context));

		public virtual IDbContextTransaction BeginTransaction()
		{
			return _context.Database.BeginTransaction();
		}

		public virtual IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel)
		{
			return _context.Database.BeginTransaction(isolationLevel);
		}

		public virtual Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
		{
			return _context.Database.BeginTransactionAsync(cancellationToken);
		}

		public virtual Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
		{
			return _context.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
		}

		public virtual int SaveChanges()
		{
			return _context.SaveChanges();
		}

		public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return _context.SaveChangesAsync(cancellationToken);
		}
	}
}
