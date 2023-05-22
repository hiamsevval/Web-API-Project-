using BookStore.Commands;
using BookStore.Data;
using BookStore.Interfaces;
using BookStore.Repository;
using MediatR;

namespace BookStore.Handlers
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, string>
    {
        
        private readonly IAuthorRepository _authorRepository;

        public DeleteAuthorCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public async Task<string> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetAuthorNoTracking(request.Id);

            if (author == null) return "Author does not exist";

            _authorRepository.DeleteAuthor(author);
            
            return "Successfully deleted";
        }
    }
}
