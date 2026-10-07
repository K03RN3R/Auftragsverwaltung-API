using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;

namespace GUI_2.DTO.Anlage
{
    public class AnlagePutDTO
    {
        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[\p{L}][\p{L}\d\s\-_/\.]+$",
            ErrorMessage = "Bezeichnung darf nur Buchstaben, Ziffern, Leerzeichen sowie -, _, ', . enthalten.")]
        public string Bezeichnung { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[A-Z0-9\-]+$",
            ErrorMessage = "AnlagenCode darf nur Großbuchstaben, Ziffern und '-' enthalten.")]
        public string AnlagenCode { get; set; }

       // public DateTime ErstelltUtc { get; set; }

       // [Required]
       // [StringLength(100)]
       // [RegularExpression(@"^[\p{L}\d\s\-'\.\\]+$",
       //ErrorMessage = "ErstelltVon darf nur Buchstaben, Ziffern, Leerzeichen sowie -, ', ., \\ enthalten.")]
       // public string GeaendertVon { get; set; } = null!;
    }
}
