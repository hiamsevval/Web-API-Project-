using AutoMapper;
using BookStore.Data;
using BookStore.DTO;
using BookStore.Interfaces;
using BookStore.Models;
using BookStore.Queries;
using MediatR;


namespace BookStore.Handlers
{
    public class GetBookHandler : IRequestHandler<GetBookQuery, BookDto>
    {
       
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public GetBookHandler(IBookRepository bookRepository, IMapper mapper)
        {
        
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<BookDto> Handle(GetBookQuery request, CancellationToken cancellationToken)
        {
            
            var book = await _bookRepository.GetBook(request.Id);
            if (book == null) return null;
            return _mapper.Map<BookDto>(book);
          
        }
    }
}
