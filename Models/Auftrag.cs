using GUI_2.Data;
using GUI_2.Data.Configuration;
using NuGet.Packaging.Signing;
using System.Drawing;

namespace GUI_2.Models
{
    public class Auftrag : IAuditable
    {
        public int Id { get; set; }
        public Guid AuftragGuid { get; set; }
        public int KundeId { get; set; }
        public int AnlageId { get; set; }
        public string Auftragsname { get; set; } 
        public string Auftragstyp { get; set; }
        public int MengeSOLL { get; set; }
        public int MengeIO { get; set; }
        public int MengeNIO { get; set; }
        public double TaktZiel { get; set; }
        public double TaktBerechnet { get; set; }
        public DateTime StartzeitUtc { get; set; }
        public DateTime EndzeitUtc { get; set; }
        public int StatusId { get; set; }
        public int Prioritaet { get; set; }
        public string Quelle { get; set; } //ursprung vom anlegen des auftrags
        public DateTime ErstelltUtc { get; set; } 
        public string ErstelltVon { get; set; } 
        public DateTime? GeaendertUtc { get; set; } 
        public string? GeaendertVon { get; set; } 

        public bool Beladen { get; set; }
        public int BeladenAnzahl { get; set; }
        public byte[]? Bilddaten { get; set; }
        
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        //Relationen
        public Kunde Kunde { get; set; } = null!;
        public Anlage Anlage { get; set; } = null!;
        public Status Status { get; set; } = null!;

        public ICollection<Fehlerbericht> Fehlerberichte { get; set; } = new List<Fehlerbericht>();
        public ICollection<AuftragSchicht> AuftragSchichten { get; set; } = new List<AuftragSchicht>();
    }
}
