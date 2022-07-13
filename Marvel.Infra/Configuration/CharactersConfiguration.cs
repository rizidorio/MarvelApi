using Marvel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marvel.Infra.Configuration
{
    public class CharactersConfiguration : IEntityTypeConfiguration<Characters>
    {
        public void Configure(EntityTypeBuilder<Characters> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.MarvelId).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.IsFavorite).HasDefaultValue(false);
        }
    }
}
