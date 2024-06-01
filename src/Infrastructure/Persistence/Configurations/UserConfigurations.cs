using Domain.Entities;
using Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
	internal sealed class UserConfigurations : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable(TableNames.Users);

			builder.HasKey(i => i.Id);

			builder.Property(i => i.Id).ValueGeneratedNever();
		}
	}
}
