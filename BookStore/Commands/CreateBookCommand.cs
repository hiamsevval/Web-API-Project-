using BookStore.DTO;
using MediatR;


namespace BookStore.Commands
{
    public class CreateBookCommand : IRequest<BookDto>
    {
        public int BookId { get; set; }
        public string Name { get; set; }
        public AuthorDto Author { get; set; }
        public string PublishYear { get; set; }

        public CreateBookCommand( string name, AuthorDto author, string publishYear)
        {
            Name = name;
            Author = author;
            PublishYear = publishYear;
        }

       
    }
}
