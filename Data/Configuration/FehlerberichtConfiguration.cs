using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GUI_2.Models;

namespace GUI_2.Data.Configuration
{
    public class FehlerberichtConfiguration : IEntityTypeConfiguration<Fehlerbericht>
    {
        public void Configure(EntityTypeBuilder<Fehlerbericht> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.FehlerberichtGuid)
                   .IsRequired()
                   .HasDefaultValueSql("NEWID()");

            builder.Property(f => f.ErstelltUtc)
                   .IsRequired();

            builder.Property(f => f.ErstelltVon)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(f => f.GeaendertUtc)
                   .IsRequired(false);

            builder.Property(f => f.GeaendertVon)
                   .IsRequired(false)
                   .HasMaxLength(100);

            builder.Property(f => f.RowVersion)
                   .IsRowVersion()
                   .IsConcurrencyToken()
                   .IsRequired();

            builder.Property(f => f.Beschreibung)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.HasOne(f => f.Auftrag)
                   .WithMany(a => a.Fehlerberichte)
                   .HasForeignKey(f => f.AuftragId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Station)
                   .WithMany(s => s.Fehlerberichte)
                   .HasForeignKey(f => f.StationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Spindel)
                   .WithMany(sp => sp.Fehlerberichte)
                   .HasForeignKey(f => f.SpindelId)
                   .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(f => f.Status)
            //       .WithMany(st => st.Fehlerberichte)
            //       .HasForeignKey(f => f.Status)
            //       .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
