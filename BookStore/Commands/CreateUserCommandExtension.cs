using BookStore.DTO;
using BookStore.Models;

namespace BookStore.Commands
{
    public static class CreateUserCommandExtension
    {
        public static BookDto CreateBook(this CreateBookCommand command)
        {
            var book = new BookDto
                (
                    command.Name,
                    command.Author,
                    command.PublishYear   
                );
            return book;
        }
        public static AuthorDto CreateAuthor(this CreateAuthorCommand command)
        {
            var author = new AuthorDto
                (
                    command.Name,
                    command.Surname
                );
            return author;
        }

    }
}
