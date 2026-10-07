using GUI_2.Data;
using NuGet.Packaging.Signing;

namespace GUI_2.Models
{
    public class Kunde : IAuditable
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Strasse { get; set; }
        public string Hausnummer { get; set; } = null!;
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

        //Relationen
        public ICollection<Auftrag> Auftraege { get; set; } = new List<Auftrag>();
        public ICollection<Anlage> Anlagen { get; set; } = new List<Anlage>();
        public ICollection<Schicht> Schichten { get; set; } = new List<Schicht>();

    }
} 
