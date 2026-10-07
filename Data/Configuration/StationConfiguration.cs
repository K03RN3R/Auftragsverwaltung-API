using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GUI_2.Models;

namespace GUI_2.Data.Configuration
{
    public class StationConfiguration : IEntityTypeConfiguration<Station>
    {
        public void Configure(EntityTypeBuilder<Station> builder) 
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.StationGuid)
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

            builder.Property(s => s.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasOne(s => s.Anlage)
                   .WithMany(a => a.Stationen)
                   .HasForeignKey(s =>s.AnlageId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.Spindeln)
                   .WithOne(sp => sp.Station)
                   .HasForeignKey(sp => sp.StationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.Fehlerberichte)
                   .WithOne(f => f.Station)
                   .HasForeignKey(f => f.StationId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
