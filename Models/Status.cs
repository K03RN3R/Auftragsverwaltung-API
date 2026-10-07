using GUI_2.Data;
using NuGet.Packaging.Signing;

namespace GUI_2.Models
{
    public class Status : IAuditable
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Bezeichnung { get; set; } = null!;
        public DateTime ErstelltUtc { get; set; }
        public string ErstelltVon { get; set; } = null!;
        public DateTime? GeaendertUtc { get; set; }
        public string? GeaendertVon { get; set; } = null!;

        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        //Relationen
        public ICollection<Auftrag> Auftraege { get; set; } = new List<Auftrag>();
        //public ICollection<Fehlerbericht> Fehlerberichte { get; set; } = new List<Fehlerbericht>();
    }
}
