using AutoMapper;
using BookStore.Commands;
using BookStore.Data;
using BookStore.DTO;
using BookStore.Interfaces;
using BookStore.Models;
using BookStore.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Handlers
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, AuthorDto>
    {
        
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public UpdateAuthorCommandHandler( IAuthorRepository authorRepository, IMapper mapper)
        {
           
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<AuthorDto> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetAuthorNoTracking(request.AuthorId);
            author.Name = request.Name;
            author.Surname=request.Surname;
            _authorRepository.UpdateAuthor(author);
            
            var response= _mapper.Map<AuthorDto>(author);
            if (author == null) return null;
            return response;
        }
    }
}
