using Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
	internal sealed class AuthorConfigurations : IEntityTypeConfiguration<Author>
	{
		public void Configure(EntityTypeBuilder<Author> builder)
		{
			builder.ToTable(TableNames.Authors);

			builder.HasKey(i => i.Id);

			builder.Property(i => i.Id).ValueGeneratedNever();

			builder.Property(i => i.FirstName).IsRequired();
			builder.Property(i => i.LastName).IsRequired();
		}
	}
}
