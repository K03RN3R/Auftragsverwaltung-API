using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;

namespace GUI_2.DTO.Station
{
    public class StationPutDTO
    {
        [Required]
        [Range(1, 9999)]
        public int AnlageId { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[\p{L}][\p{L}\d\s\-_/\.]+$",
            ErrorMessage = "Name enthält unzulässige Zeichen.")]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        //[Required]
        //[StringLength(100)]
        //[RegularExpression(@"^[\p{L}\d\s\-'\.\\]+$",
        //    ErrorMessage = "GeaendertVon enthält unzulässige Zeichen.")]
        //public string GeaendertVon { get; set; } = null!;
    }
}
