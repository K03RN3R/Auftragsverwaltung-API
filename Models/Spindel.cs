using GUI_2.Data;
using NuGet.Packaging.Signing;

namespace GUI_2.Models
{
    public class Spindel : IAuditable
    {
        public int Id { get; set; }
        public Guid SpindelGuid { get; set; }
        public int StationId { get; set; }
        public int Nummer { get; set; }
        public string Bezeichnung { get; set; } 
        public bool IsActive { get; set; }
        public DateTime ErstelltUtc { get; set; }
        public string ErstelltVon { get; set; } = null!;
        public DateTime? GeaendertUtc { get; set; }
        public string? GeaendertVon { get; set; } = null!;
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        //Relationen
        public Station Station { get; set; } = null!;
        public ICollection<Fehlerbericht> Fehlerberichte { get; set; } = new List<Fehlerbericht>();
    }
}

