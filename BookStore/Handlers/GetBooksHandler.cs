using AutoMapper;
using BookStore.Data;
using BookStore.DTO;
using BookStore.Interfaces;
using BookStore.Models;
using BookStore.Queries;
using MediatR;


namespace BookStore.Handlers
{
    public class GetBooksHandler : IRequestHandler<GetBooksQuery, IEnumerable<BookDto>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public GetBooksHandler( IBookRepository bookRepository, IMapper mapper)
        {
           
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Book> books;
            if(request.Str!=null)
            {
                request.Str = request.Str.Trim().ToUpper();
                books = await _bookRepository.GetBooks(x => x.Name.Trim().ToUpper().Contains(request.Str));
            }
            else { books = await _bookRepository.GetBooks(null); }
            
            return _mapper.Map<List<BookDto>>(books);
        
        }
    }
}