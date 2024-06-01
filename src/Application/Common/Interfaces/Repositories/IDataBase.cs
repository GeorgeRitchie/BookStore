using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Application.Common.Interfaces.Repositories
{
	public interface IDataBase
	{
		IDbContextTransaction BeginTransaction();
		Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
		IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel);
		Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default);
		int SaveChanges();
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
