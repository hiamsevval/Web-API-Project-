using BookStore.DTO;
using BookStore.Models;
using MediatR;

namespace BookStore.Queries
{
    public class GetBookQuery : IRequest<BookDto>
    {
        public int Id { get; set; }

        public GetBookQuery(int id)
        {
            Id = id;
        }
    }
}
