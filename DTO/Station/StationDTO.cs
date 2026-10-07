using NuGet.Packaging.Signing;

namespace GUI_2.DTO.Station
{
    public class StationDTO
    {
        public int AnlageId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string GeaendertVon { get; set; } = null!;
    }
}
