using GUI_2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GUI_2.Data.Configuration
{
    public class AnlageConfiguration : IEntityTypeConfiguration<Anlage> 
    {
        public void Configure(EntityTypeBuilder<Anlage> builder) 
        {
            builder.HasKey(a => a.Id);


            builder.Property(a => a.AnlageGuid)
                   .IsRequired()
                   .HasDefaultValueSql("NEWID()");

            builder.Property(a => a.Bezeichnung)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(a => a.ErstelltUtc)
                   .IsRequired();

            builder.Property(a => a.ErstelltVon)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.GeaendertUtc)
                   .IsRequired(false); // Nullable

            builder.Property(a => a.GeaendertVon)
                   .IsRequired(false)
                   .HasMaxLength(100);

            builder.Property(a => a.RowVersion)
                   .IsRowVersion()
                   .IsConcurrencyToken()
                   .IsRequired();

            builder.HasOne(a => a.Kunde)
                   .WithMany(k => k.Anlagen)
                   .HasForeignKey(a => a.KundeId)
                   .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasMany(a => a.Stationen)
                   .WithOne(s => s.Anlage)
                   .HasForeignKey(s => s.AnlageId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Auftraege)
                   .WithOne(o => o.Anlage)
                   .HasForeignKey(o => o.AnlageId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
