using NuGet.Packaging.Signing;

namespace GUI_2.DTO.Schicht
{
    public class SchichtGetDTO
    {
        public int Id { get; set; }
        public Guid SchichtGuid { get; set; }
        public int KundeId { get; set; }
        public string Name { get; set; }
        public TimeOnly StartzeitTag { get; set; }
        public TimeOnly EndzeitTag { get; set; }
        public bool Mitternachtsarbeit { get; set; } 
        public DateTime ErstelltUtc { get; set; }
        public string ErstelltVon { get; set; } = null!;
        public DateTime? GeaendertUtc { get; set; }
        public string? GeaendertVon { get; set; } = null!;
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
