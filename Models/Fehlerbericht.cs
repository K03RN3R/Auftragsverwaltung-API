using GUI_2.Data;
using NuGet.Packaging.Signing;

namespace GUI_2.Models
{
    public class Fehlerbericht : IAuditable
    {
        public int Id { get; set; }
        public Guid FehlerberichtGuid { get; set; }
        public int StationId { get; set; }
        public int AuftragId { get; set; }
        public int SpindelId { get; set; }
        public int StatusId { get; set; }
        public int Spindelnummer { get; set; }
        public string Code { get; set; } 
        public string Status { get; set; }
        public string Titel { get; set; }
        public string Beschreibung { get; set; }
        public string Schwere { get; set; }
        public DateTime AufgetretenUtc { get; set; }
        public DateTime BestaetigtUtc { get; set; }
        public DateTime BehobenUtc { get; set; }
        public string CustomData { get; set; }
        public DateTime ErstelltUtc { get; set; }
        public string ErstelltVon { get; set; } = null!;
        public DateTime? GeaendertUtc { get; set; }
        public string? GeaendertVon { get; set; } = null!;
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        //Relationen
        public Auftrag Auftrag { get; set; } = null!;
        public Station Station { get; set; } = null!;
        public Spindel Spindel { get; set; } = null!;
        //public Status Status { get; set; } /*= null!*/;


    }
}
