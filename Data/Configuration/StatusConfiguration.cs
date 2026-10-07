using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GUI_2.Models;

namespace GUI_2.Data.Configuration
{
    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Guid)
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

            builder.Property(s => s.Bezeichnung)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasMany(s => s.Auftraege)
                   .WithOne(a => a.Status)
                   .HasForeignKey(a => a.StatusId)
                   .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(s => s.Fehlerberichte)
            //       .WithOne(f => f.Status)
            //       .HasForeignKey(f => f.StatusId)
            //       .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
