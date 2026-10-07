using GUI_2.Data;
using NuGet.Packaging.Signing;

namespace GUI_2.Models
{
    public class Station : IAuditable
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

        //Relationen
        public Anlage Anlage { get; set; } = null!;

        public ICollection<Spindel> Spindeln { get; set; } = new List<Spindel>();
        public ICollection<Fehlerbericht> Fehlerberichte { get; set; } = new List<Fehlerbericht>();
    }
}
