using NuGet.Packaging.Signing;

namespace GUI_2.DTO.Anlage
{
    public class AnlageDTO
    {
        public int KundeId { get; set; }
        public string Bezeichnung { get; set; }
        public string AnlagenCode { get; set; }
        public string GeaendertVon { get; set; } = null!;
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
