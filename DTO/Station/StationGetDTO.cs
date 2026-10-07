using NuGet.Packaging.Signing;

namespace GUI_2.DTO.Station
{
    public class StationGetDTO
    {
        public int Id { get; set; }
        public Guid StationGuid { get; set; }
        public int AnlageId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime ErstelltUtc { get; set; }
        public string ErstelltVon { get; set; } = null!;
        public DateTime? GeaendertUtc { get; set; }
        public string? GeaendertVon { get; set; } = null!;
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
