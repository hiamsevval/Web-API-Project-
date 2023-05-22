using BookStore.Models;
using MediatR;

namespace BookStore.Commands
{
    public class DeleteBookCommand : IRequest<string>
    {
        public int bookId;

        public DeleteBookCommand(int id)
        {
            bookId = id;
        }
    }
}
