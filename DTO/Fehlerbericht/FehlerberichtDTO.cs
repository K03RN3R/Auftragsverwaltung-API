using NuGet.Packaging.Signing;

namespace GUI_2.DTO.Fehlerbericht
{
    public class FehlerberichtDTO
    {
        public int StationId { get; set; }
        public int AuftragId { get; set; }
        public int SpindelId { get; set; }
        public int Spindelnummer { get; set; }
        public string Code { get; set; }
        public string Titel { get; set; }
        public string Beschreibung { get; set; }
        public string Schwere { get; set; }
        public string Status { get; set; }
        public string CustomData { get; set; }
        public string GeaendertVon { get; set; } = null!;
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
