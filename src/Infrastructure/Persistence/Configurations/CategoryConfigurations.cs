using Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
	internal sealed class CategoryConfigurations : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.ToTable(TableNames.Categories);

			builder.HasKey(i => i.Id);

			builder.Property(i => i.Id).ValueGeneratedNever();

			builder.Property(i => i.Name).IsRequired();
		}
	}
}
