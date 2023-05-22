using BookStore.DTO;
using BookStore.Models;
using MediatR;

namespace BookStore.Queries
{
    public class GetBooksQuery : IRequest<IEnumerable<BookDto>>
    {
        public string Str { get; set; }

        public GetBooksQuery(string str)
        {
            Str = str; 
        }
    }
}
