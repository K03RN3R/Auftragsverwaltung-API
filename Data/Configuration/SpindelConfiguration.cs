using GUI_2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GUI_2.Data.Configuration
{
    public class SpindelConfiguration : IEntityTypeConfiguration<Spindel>
    {
        public void Configure(EntityTypeBuilder<Spindel> builder) 
        {
            builder.HasKey(sp => sp.Id);

            builder.Property(sp => sp.SpindelGuid)
                   .IsRequired()
                   .HasDefaultValueSql("NEWID()");

            builder.Property(sp => sp.ErstelltUtc).IsRequired();
            builder.Property(sp => sp.ErstelltVon).IsRequired().HasMaxLength(100);
            builder.Property(sp => sp.GeaendertUtc).IsRequired(false);
            builder.Property(sp => sp.GeaendertVon).IsRequired(false).HasMaxLength(100);

            builder.Property(sp => sp.RowVersion)
                   .IsRowVersion()
                   .IsConcurrencyToken()
                   .IsRequired();

            builder.Property(sp => sp.Bezeichnung)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasOne(sp => sp.Station)
                   .WithMany(s => s.Spindeln)
                   .HasForeignKey(sp => sp.StationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(sp => sp.Fehlerberichte)
                   .WithOne(fb => fb.Spindel)
                   .HasForeignKey(fb => fb.SpindelId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
