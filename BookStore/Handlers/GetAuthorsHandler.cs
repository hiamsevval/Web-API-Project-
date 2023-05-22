using AutoMapper;
using BookStore.Data;
using BookStore.DTO;
using BookStore.Interfaces;
using BookStore.Models;
using BookStore.Queries;
using BookStore.Repository;
using MediatR;

namespace BookStore.Handlers
{
    public class GetAuthorsHandler : IRequestHandler<GetAuthorsQuery, IEnumerable<AuthorDto>>
    {
        
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public GetAuthorsHandler( IAuthorRepository authorRepository, IMapper mapper)
        {
            
            _authorRepository = authorRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<AuthorDto>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Author> authors;
            if (request.Str != null)
            {
                request.Str = request.Str.Trim().ToUpper();
                authors = await _authorRepository.GetAuthors(x => x.Name.Trim().ToUpper().Contains(request.Str));
            }
            else { authors = await _authorRepository.GetAuthors(null); }
            
            return _mapper.Map<List<AuthorDto>>(authors);
        }
    }
}
