using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GUI_2.Models;

namespace GUI_2.Data.Configuration
{
    public class AuftragSchichtConfiguration : IEntityTypeConfiguration<AuftragSchicht>
    {
        public void Configure(EntityTypeBuilder<AuftragSchicht> builder)
        {
            builder.HasKey(x => new { x.AuftragId, x.SchichtId });

            builder.HasOne(x => x.Auftrag)
                   .WithMany (a => a.AuftragSchichten)
                   .HasForeignKey(x => x.AuftragId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Schicht)
                   .WithMany(s => s.AuftragSchichten)
                   .HasForeignKey(x => x.SchichtId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
