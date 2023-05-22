using AutoMapper;
using BookStore.Data;
using BookStore.DTO;
using BookStore.Interfaces;
using BookStore.Queries;
using MediatR;

namespace BookStore.Handlers
{

    public class GetAuthorHandler : IRequestHandler<GetAuthorQuery, AuthorDto>
    {
        
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public GetAuthorHandler(IAuthorRepository authorRepository, IMapper mapper)
        {
            
            _authorRepository = authorRepository;
            _mapper = mapper;
        }
    
        public async Task<AuthorDto> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetAuthor(request.Id);
            if (author == null) return null;
            return _mapper.Map<AuthorDto>(author);
        }
    }
}
