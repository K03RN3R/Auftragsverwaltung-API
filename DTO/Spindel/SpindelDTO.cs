using NuGet.Packaging.Signing;

namespace GUI_2.DTO.Spindel
{
    public class SpindelDTO
    {
        public int StationId { get; set; }
        public int Nummer { get; set; }
        public string Bezeichnung { get; set; }
        public bool IsActive { get; set; }
        public string GeaendertVon { get; set; } = null!;
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
