using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GUI_2.Models;

namespace GUI_2.Data.Configuration
{
    public class AuftragConfiguration : IEntityTypeConfiguration<Auftrag>
    {
        public void Configure(EntityTypeBuilder<Auftrag> builder) 
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.AuftragGuid)
                   .IsRequired()
                   .HasDefaultValueSql("NEWID()");

            builder.Property(a => a.ErstelltUtc)
                   .IsRequired();

            builder.Property(a => a.ErstelltVon)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.GeaendertUtc)
                   .IsRequired(false);

            builder.Property(a => a.GeaendertVon)
                   .IsRequired(false)
                   .HasMaxLength(100);

            builder.Property(a => a.RowVersion)
                   .IsRowVersion()
                   .IsConcurrencyToken()
                   .IsRequired();

            builder.Property(a => a.Auftragsname)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasOne(a => a.Kunde)
                   .WithMany(k => k.Auftraege)
                   .HasForeignKey(a => a.KundeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Anlage)
                   .WithMany(an => an.Auftraege)
                   .HasForeignKey(a => a.AnlageId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Fehlerberichte)
                   .WithOne(f => f.Auftrag)
                   .HasForeignKey(f => f.AuftragId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(a => a.Status)
                   .WithMany(s => s.Auftraege)
                   .HasForeignKey(a => a.StatusId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
