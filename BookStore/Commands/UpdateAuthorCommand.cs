using BookStore.DTO;
using MediatR;

namespace BookStore.Commands
{
    public class UpdateAuthorCommand : IRequest<AuthorDto>
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public UpdateAuthorCommand(int id, string name, string surname)
        {
            AuthorId= id;
            Name= name;
            Surname = surname;
        }
    }
}
