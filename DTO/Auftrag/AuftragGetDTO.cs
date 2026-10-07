using NuGet.Packaging.Signing;
using System.Drawing;

namespace GUI_2.DTO.Auftrag
{
    public class AuftragGetDTO
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
        public int StatusId { get; set; }
        public int Prioritaet { get; set; }
        public string Quelle { get; set; } //was ist damit gemeint?
        public DateTime ErstelltUtc { get; set; } 
        public string ErstelltVon { get; set; } = null!;
        public DateTime? GeaendertUtc { get; set; }
        public string? GeaendertVon { get; set; } = null!;
        public bool Beladen { get; set; }
        public int BeladenAnzahl { get; set; }
        public byte[] Bilddaten { get; set; }
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
