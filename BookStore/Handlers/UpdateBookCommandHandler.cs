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
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookDto>
    {
        
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public UpdateBookCommandHandler( IBookRepository bookRepository, IMapper mapper)
        {
           
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<BookDto> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetBookNoTracking(request.BookId);
            var updatedBook = _mapper.Map<BookDto>(book);
            updatedBook.Name = request.Name;
            updatedBook.Author= request.Author;
            updatedBook.PublishYear = request.PublishYear;
            var save = _mapper.Map<Book>(updatedBook);
            _bookRepository.UpdateBook(save);
           
            return updatedBook;
        }
    }
}
