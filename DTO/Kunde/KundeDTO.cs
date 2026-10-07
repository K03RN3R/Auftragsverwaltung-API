namespace GUI_2.DTO.Kunde
{
    public class KundeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Strasse { get; set; }
        public int Hausnummer { get; set; }
        public int PLZ { get; set; }
        public string Ort { get; set; }
        public string Land { get; set; }
        public string Email { get; set; }
        public string Telefonnummer { get; set; }
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    }

}
