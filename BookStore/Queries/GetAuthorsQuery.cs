using BookStore.DTO;
using MediatR;

namespace BookStore.Queries
{
    public class GetAuthorsQuery : IRequest<IEnumerable<AuthorDto>>
    {
        public string Str { get; set; }

        public GetAuthorsQuery(string str)
        {
            Str = str;
        }
    }
}
