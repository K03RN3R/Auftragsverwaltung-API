using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace GUI_2.DTO.Auftrag
{
    public class AuftragPutDTO
    {
        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[\p{L}][\p{L}\d\s\-_/\.]+$",
        ErrorMessage = "Auftragsname darf nur Buchstaben, Ziffern, Leerzeichen sowie '-', '_', '/' und '.' enthalten.")]
        public string Auftragsname { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^(FERTIGUNG|NACHARBEIT|TEST|MUSTER)$", ErrorMessage = "Auftragstyp ist ungültig.")]
        public string Auftragstyp { get; set; }

        [Required]
        [Range(1, 9999, ErrorMessage = "MengeSOLL muss größer 0 sein.")]
        public int MengeSOLL { get; set; }

        [Required]
        [Range(1, 9999, ErrorMessage = "MengeIO muss größer 0 sein.")]
        public int MengeIO { get; set; }

        [Required]
        [Range(1, 9999, ErrorMessage = "MengeNIO muss größer 0 sein.")]
        public int MengeNIO { get; set; }

        [Required]
        [Range(1, 9.99, ErrorMessage = "Wert muss zwischen 0.00 und 9.99 liegen.")]
        public double TaktZiel { get; set; }
        [Required]
        [Range(1, 9.99, ErrorMessage = "Wert muss zwischen 0.00 und 9.99 liegen.")]
        public double TaktBerechnet { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "StatusId muss zwischen 1 und 5 liegen.")]
        public int StatusId { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Priorität muss zwischen 1 und 10 liegen.")]
        public int Prioritaet { get; set; }
        public string Quelle { get; set; } //was ist damit gemeint?

        [Required]
        public bool Beladen { get; set; }

        [Required]
        [Range(1, 9999, ErrorMessage = "MengeSOLL muss größer 0 sein.")]
        public int BeladenAnzahl { get; set; }
        public byte[]? Bilddaten { get; set; }
        //public DateTime GeaendertUtc { get; set; }
        //public string GeaendertVon { get; set; }
    }
}

