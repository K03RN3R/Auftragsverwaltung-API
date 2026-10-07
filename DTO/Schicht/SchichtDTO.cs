using NuGet.Packaging.Signing;

namespace GUI_2.DTO.Schicht
{
    public class SchichtDTO
    {
        public int KundeId { get; set; }
        public string Name { get; set; }
        public TimeOnly StartzeitTag { get; set; }
        public TimeOnly EndzeitTag { get; set; }
        public bool Mitternachtsarbeit { get; set; } 
        public string GeaendertVon { get; set; } = null!;
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
