using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GUI_2.Models;

namespace GUI_2.Data.Configuration
{
    public class SchichtConfiguration : IEntityTypeConfiguration<Schicht>
    {
        public void Configure(EntityTypeBuilder<Schicht> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(s => s.SchichtGuid)
                   .IsRequired()
                   .HasDefaultValueSql("NEWID()");

            builder.Property(s => s.ErstelltUtc).IsRequired();
            builder.Property(s => s.ErstelltVon).IsRequired().HasMaxLength(100);
            builder.Property(s => s.GeaendertUtc).IsRequired(false);
            builder.Property(s => s.GeaendertVon).IsRequired(false).HasMaxLength(100);

            builder.Property(s => s.RowVersion)
                   .IsRowVersion()
                   .IsConcurrencyToken()
                   .IsRequired();

            builder.HasOne(s => s.Kunde)
                   .WithMany(k => k.Schichten)
                   .HasForeignKey(s => s.KundeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.AuftragSchichten)
                   .WithOne(x => x.Schicht)
                   .HasForeignKey(x => x.SchichtId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
