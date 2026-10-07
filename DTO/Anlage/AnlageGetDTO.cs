using NuGet.Packaging.Signing;

namespace GUI_2.DTO.Anlage
{
    public class AnlageGetDTO
    {
        public int Id { get; set; }
        public Guid AnlageGuid { get; set; }
        public int KundeId { get; set; }
        public string Bezeichnung { get; set; }
        public string AnlagenCode { get; set; }
        public DateTime ErstelltUtc { get; set; }
        public string ErstelltVon { get; set; } = null!;
        public DateTime? GeaendertUtc { get; set; }
        public string? GeaendertVon { get; set; } = null!;
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
