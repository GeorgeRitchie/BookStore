using Infrastructure.Common;
using Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistence
{
	// add-migration Init -context AppDbContext -o Persistence/Migrations
	// update-database -context AppDbContext
	// migration -context AppDbContext
	// remove-migration -context AppDbContext
	// drop-database -context AppDbContext
	internal class AppDbContext(DbContextOptions<AppDbContext> options,
								 ILogger<AppDbContext> logger,
								 IOptions<InfrastructureSettings> infrastructureSettings) : DbContext(options)
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (infrastructureSettings.Value.IsDevelopmentEnvironment)
				optionsBuilder.LogTo(message =>
										{
											System.Diagnostics.Debug.WriteLine(message);
											logger.LogInformation(message);
										},
									new[] { RelationalEventId.CommandExecuted });
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.HasDefaultSchema(Schemas.BookStore);

			modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
		}
	}
}
