
namespace BookStore.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Author Author { get; set; }
        public string PublishYear { get; set; }

        /*
        public Book( string name, Author author, string publishYear)
        {
            Name = name;
            Author = author;
            PublishYear = publishYear;
        }*/
    }
}
