using GUI_2.Data;
using NuGet.Packaging.Signing;

namespace GUI_2.Models
{
    public class Anlage : IAuditable
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

        //Relationen
        public Kunde Kunde { get; set; } = null!;
        public ICollection<Station> Stationen { get; set; } = new List<Station>();
        public ICollection<Auftrag> Auftraege { get; set; } = new List<Auftrag>();
    }
}


