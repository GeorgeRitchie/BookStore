using Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
	internal sealed class BookConfigurations : IEntityTypeConfiguration<Book>
	{
		public void Configure(EntityTypeBuilder<Book> builder)
		{
			builder.ToTable(TableNames.Books);

			builder.HasKey(i => i.Id);

			builder.Property(i => i.Id).ValueGeneratedNever();

			builder.Property(i => i.Title).IsRequired();

			builder.HasIndex(i => i.ISBN).IsUnique();

			builder.Property(i => i.Price).HasPrecision(20, 4);

			builder.HasMany(i => i.Categories).WithMany(c => c.Books);
			builder.HasMany(i => i.Authors).WithMany(a => a.Books);
		}
	}
}
