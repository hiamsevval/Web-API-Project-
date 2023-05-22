using AutoMapper;
using BookStore.Commands;
using BookStore.Data;
using BookStore.Interfaces;
using BookStore.Models;
using MediatR;

namespace BookStore.Handlers
{
    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, Author>
    {
        private readonly IMapper _mapper;
        private readonly IAuthorRepository _authorRepository;
        public CreateAuthorCommandHandler( IMapper mapper, IAuthorRepository authorRepository)
        {
            _mapper = mapper;
            _authorRepository=authorRepository;
        }
        public async Task<Author> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = request.CreateAuthor();
            var authorResponse = _mapper.Map<Author>(author);

            _authorRepository.CreateAuthor(authorResponse);
           
            return authorResponse;
        }
    }
}
