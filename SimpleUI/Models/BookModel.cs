namespace SimpleUI.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AuthorModel Author { get; set; }
        public string PublishYear { get; set; }
    }
}
