using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;

namespace GUI_2.DTO.Schicht
{
    public class SchichtPutDTO
    {
        [Required]
        [Range(1, 9999)]
        public int KundeId { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[\p{L}][\p{L}\d\s\-_/\.]+$",
            ErrorMessage = "Name enthält unzulässige Zeichen.")]
        public string Name { get; set; }

        [Required]
        public TimeOnly StartzeitTag { get; set; }

        [Required]
        public TimeOnly EndzeitTag { get; set; }

        // typischer Ja/Nein-Wert. Empfehlung: bool statt string
        [Required]
        public bool Mitternachtsarbeit { get; set; }

        //public DateTime GeaendertUtc { get; set; }

        //[Required]
        //[StringLength(100)]
        //[RegularExpression(@"^[\p{L}\d\s\-'\.\\]+$",
        //    ErrorMessage = "GeaendertVon enthält unzulässige Zeichen.")]
        //public string GeaendertVon { get; set; } = null!;
    }
}
