using BookStore.Models;
using MediatR;

namespace BookStore.Commands
{
    public class CreateAuthorCommand : IRequest<Author>
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public CreateAuthorCommand(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }
    }
}
