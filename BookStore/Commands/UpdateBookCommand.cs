using BookStore.DTO;
using MediatR;

namespace BookStore.Commands
{
    public class UpdateBookCommand : IRequest<BookDto>
    {
        public int BookId { get; set; }
        public string Name { get; set; }
        public AuthorDto Author { get; set; }
        public string PublishYear { get; set; }

        public UpdateBookCommand(int id, string name, AuthorDto author, string publishYear)
        {
            BookId = id;
            Name = name;
            Author = author;
            PublishYear = publishYear;
        }
    }
}
