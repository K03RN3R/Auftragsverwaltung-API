namespace GUI_2.Data
{
    public interface IAuditable
    {
        DateTime ErstelltUtc { get; set; }
        string ErstelltVon { get; set; }
        DateTime? GeaendertUtc { get; set; }
        string? GeaendertVon { get; set; }
    }
}
