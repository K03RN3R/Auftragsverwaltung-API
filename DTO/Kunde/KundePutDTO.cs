using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;

namespace GUI_2.DTO.Kunde
{
    public class KundePutDTO
    {
        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[\p{L}][\p{L}\d\s\-_/\.]+$", ErrorMessage = "Name darf nur Buchstaben, Leerzeichen, '-', ''' und '.' enthalten.")]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[\p{L}][\p{L}\d\s\-_/\.]+$", ErrorMessage = "Straße darf nur Buchstaben, Ziffern, Leerzeichen sowie '.', '-' und '/' enthalten.")]
        public string Strasse { get; set; }

        [Required]
        [StringLength(10)]
        [RegularExpression(@"^\d{1,4}[a-zA-Z]?$",ErrorMessage = "Hausnummer muss aus 1–4 Ziffern mit optionalem Buchstaben bestehen (z. B. 12 oder 12A).")]
        public string Hausnummer { get; set; }

        [StringLength(5, MinimumLength = 5)]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Postleitzahl muss aus genau 5 Ziffern bestehen.")] //wechsel auf string, da bei int die 0 vorne wegfällt. so sind 0 plz möglich
        public string PLZ { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[\p{L}][\p{L}\d\s\-_/\.]+$", ErrorMessage = "Ort darf nur Buchstaben, Leerzeichen, '-', ''' und '.' enthalten.")]
        public string Ort { get; set; }

        [Required]
        [StringLength(2)]
        [RegularExpression(@"^[A-Z]{2}$",ErrorMessage = "Land muss als ISO-Ländercode mit zwei Großbuchstaben angegeben werden (z. B. DE, AT, CH).")]
        public string Land { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(16)] // + und bis zu 15 Ziffern
        [RegularExpression(@"^\+[1-9]\d{7,14}$",ErrorMessage = "Telefonnummer muss im E.164 Format sein, z. B. +4915123456789.")]
        public string Telefonnummer { get; set; }

        //public DateTime GeaendertUtc { get; set; }
        //public string GeaendertVon { get; set; }
    }
}
