using NuGet.Packaging.Signing;

namespace GUI_2.DTO.Kunde
{
    public class KundeGetDTO
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Strasse { get; set; }
        public string Hausnummer { get; set; }
        public string PLZ { get; set; }
        public string Ort { get; set; }
        public string Land { get; set; }
        public string Email { get; set; }
        public string Telefonnummer { get; set; }
        public DateTime ErstelltUtc { get; set; }
        public string ErstelltVon { get; set; }
        public DateTime? GeaendertUtc { get; set; }
        public string? GeaendertVon { get; set; }
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
