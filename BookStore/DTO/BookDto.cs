using BookStore.Models;
using System.Text.Json.Serialization;

namespace BookStore.DTO
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AuthorDto Author { get; set; }
        public string PublishYear { get; set; }
        
        public BookDto(string name, AuthorDto author, string publishYear)
        {
            Name = name;
            Author= author;
            PublishYear = publishYear;
        }
     
    }
}
