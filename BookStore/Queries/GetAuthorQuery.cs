using BookStore.DTO;
using MediatR;

namespace BookStore.Queries
{
    public class GetAuthorQuery : IRequest<AuthorDto>
    {

        public int Id { get; set; }

        public GetAuthorQuery(int id)
        {
            Id = id;
        }

    }
}
