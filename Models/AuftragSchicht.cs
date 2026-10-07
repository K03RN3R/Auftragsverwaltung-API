using GUI_2.Data;

namespace GUI_2.Models
{
    public class AuftragSchicht
    {
        public int AuftragId { get; set; }
        public Auftrag Auftrag { get; set; } = null!;

        public int SchichtId { get; set; }
        public Schicht Schicht { get; set; } = null!;
    }
}
