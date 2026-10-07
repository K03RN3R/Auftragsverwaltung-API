using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;

namespace GUI_2.DTO.Status
{
    public class StatusCreateDTO
    {
        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[\p{L}][\p{L}\d\s\-_/\.]+$",
            ErrorMessage = "Bezeichnung enthält unzulässige Zeichen.")]
        public string Bezeichnung { get; set; } = null!;

        //[Required]
        //[StringLength(100)]
        //[RegularExpression(@"^[\p{L}\d\s\-'\.\\]+$",
        //    ErrorMessage = "ErstelltVon enthält unzulässige Zeichen.")]
        //public string ErstelltVon { get; set; } = null!;
    }
}
