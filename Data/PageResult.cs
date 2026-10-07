namespace GUI_2.Data
{
    public class PageResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        //gibt es noch eine oder mehrere seiten oder ist das die letzte seite bool.hasnext?
    }
}
