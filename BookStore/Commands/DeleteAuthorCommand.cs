using MediatR;

namespace BookStore.Commands
{
    public class DeleteAuthorCommand : IRequest<string>
    {
        public int Id { get; set; }

        public DeleteAuthorCommand(int id)
        {
            Id = id;
        }
    }
}
