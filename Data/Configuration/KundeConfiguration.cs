using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GUI_2.Models;

namespace GUI_2.Data.Configuration
{

        public class KundeConfiguration : IEntityTypeConfiguration<Kunde>
        {
            public void Configure(EntityTypeBuilder<Kunde> builder)
            {

             //Primärschlüssel
             builder.HasKey(k => k.Id);

            builder.Property(k => k.Id)
                           .ValueGeneratedOnAdd(); // Identity Spalte

            // GUID wird automatisch per NEWID() generiert
            builder.Property(k => k.Guid)
                .HasDefaultValueSql("NEWID()")
                .IsRequired();

            // Pflichtfelder
            builder.Property(k => k.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(k => k.ErstelltUtc)
                .IsRequired();

            builder.Property(k => k.ErstelltVon)
                .IsRequired()
                .HasMaxLength(100);

            // Optional Nullable Felder: Für Änderinfos
            builder.Property(k => k.GeaendertUtc)
                .IsRequired(false);

            builder.Property(k => k.GeaendertVon)
                .IsRequired(false)
                .HasMaxLength(100);

            // Concurrency Handling (für ETag)
            builder.Property(k => k.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken()
                .IsRequired();

            builder.HasMany(k => k.Auftraege)
                       .WithOne(a => a.Kunde)
                       .HasForeignKey(a => a.KundeId)
                       .OnDelete(DeleteBehavior.Restrict); 

             builder.HasMany(k => k.Anlagen)
                       .WithOne(a => a.Kunde)
                       .HasForeignKey(a => a.KundeId)
                       .OnDelete(DeleteBehavior.Restrict);

             builder.HasMany(k => k.Schichten)
                       .WithOne(s => s.Kunde)
                       .HasForeignKey(s => s.KundeId)
                       .OnDelete(DeleteBehavior.Restrict);
            }
        }
    }

