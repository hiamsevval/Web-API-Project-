using AutoMapper;
using BookStore.Commands;
using BookStore.Data;
using BookStore.DTO;
using BookStore.Interfaces;
using BookStore.Models;
using BookStore.Repository;
using MediatR;


namespace BookStore.Handlers
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookDto>
    {
        private readonly DataContext _dataContext;
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        

        public CreateBookCommandHandler(IBookRepository bookRepository, IAuthorRepository authorRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            // _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<BookDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {/*
            var book = request.CreateBook();
            var bookResponse = _mapper.Map<Book>(book);

            bookResponse.Author = _dataContext.Authors.Where(a=>a.Id==book.Author.Id).FirstOrDefault();
            
            
            await _dataContext.Books.AddAsync(bookResponse);
            await _dataContext.SaveChangesAsync();
            */
            var book = request.CreateBook();
            var bookResponse = _mapper.Map<Book>(book);

            bookResponse.Author = await _authorRepository.GetAuthor(request.Author.Id);

            _bookRepository.CreateBook(bookResponse);

            return book;
        }
    }
}
