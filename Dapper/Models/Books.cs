namespace DapperAPI.Models
{
    public class Books
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Publisher { get; set; }
        public int pages { get; set; }
        public string ShelfLocation { get; set; }
    }
}
