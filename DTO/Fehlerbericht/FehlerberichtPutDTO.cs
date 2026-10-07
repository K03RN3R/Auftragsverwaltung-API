using System.ComponentModel.DataAnnotations;

namespace GUI_2.DTO.Fehlerbericht
{
    public class FehlerberichtPutDTO
    {
        [Required]
        [Range(1, 9999)]
        public int StationId { get; set; }

        [Required]
        [Range(1, 9999)]
        public int AuftragId { get; set; }

        [Required]
        [Range(1, 9999)]
        public int SpindelId { get; set; }

        [Required]
        [Range(1, 9999)]
        public int Spindelnummer { get; set; }

        // kurzer technischer Code, z. B. FEHLER_01
        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Z0-9_\-]+$",
            ErrorMessage = "Code darf nur Großbuchstaben, Ziffern, '_' und '-' enthalten.")]
        public string Code { get; set; }

        // Titel = klarer Text
        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[\p{L}][\p{L}\d\s\-_/\.]+$",
            ErrorMessage = "Titel enthält unzulässige Zeichen.")]
        public string Titel { get; set; }

        // Beschreibung = längerer Freitext, nicht unnötig einschränken
        [Required]
        [StringLength(1000)]
        public string Beschreibung { get; set; }

        // Schwere = z. B. LOW/MEDIUM/HIGH oder 1/2/3
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^(LOW|MEDIUM|HIGH|1|2|3)$",
            ErrorMessage = "Schwere muss LOW, MEDIUM, HIGH oder 1–3 sein.")]
        public string Schwere { get; set; }

        // Status = definierter Zustandswert
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^(OFFEN|IN_BEARBEITUNG|ERLEDIGT|FEHLER)$",
            ErrorMessage = "Status ist ungültig.")]
        public string Status { get; set; }
        public string CustomData { get; set; }

        //public DateTime GeaendertUtc { get; set; }

        //[Required]
        //[StringLength(100)]
        //[RegularExpression(@"^[\p{L}\d\s\-'\.\\]+$",
        //    ErrorMessage = "GeaendertVon enthält unzulässige Zeichen.")]
        //public string GeaendertVon { get; set; } = null!;
    }
}
