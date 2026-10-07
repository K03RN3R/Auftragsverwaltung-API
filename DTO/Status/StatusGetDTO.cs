using NuGet.Packaging.Signing;

namespace GUI_2.DTO.Status
{
    public class StatusGetDTO
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Bezeichnung { get; set; } = null!;
        public DateTime ErstelltUtc { get; set; }
        public string ErstelltVon { get; set; } = null!;
        public DateTime? GeaendertUtc { get; set; }
        public string? GeaendertVon { get; set; } = null!;

        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
