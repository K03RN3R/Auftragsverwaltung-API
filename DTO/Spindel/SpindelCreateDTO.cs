using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;

namespace GUI_2.DTO.Spindel
{
    public class SpindelCreateDTO
    {
        [Required]
        [Range(1, 9999)]
        public int StationId { get; set; }

        [Required]
        [Range(1, 9999)]
        public int Nummer { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[\p{L}][\p{L}\d\s\-_/\.]+$",
            ErrorMessage = "Bezeichnung enthält unzulässige Zeichen.")]
        public string Bezeichnung { get; set; }

        [Required]
        public bool IsActive { get; set; }

        //[Required]
        //public DateTime ErstelltUtc { get; set; }

        //[Required]
        //[StringLength(100)]
        //[RegularExpression(@"^[\p{L}\d\s\-'\.\\]+$",
        //    ErrorMessage = "ErstelltVon enthält unzulässige Zeichen.")]
        //public string ErstelltVon { get; set; } = null!;
    }
}
