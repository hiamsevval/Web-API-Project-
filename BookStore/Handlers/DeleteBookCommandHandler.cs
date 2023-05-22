using AutoMapper;
using BookStore.Commands;
using BookStore.Data;
using BookStore.DTO;
using BookStore.Interfaces;
using BookStore.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Handlers
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, string>
    {
        private readonly IBookRepository _bookRepository;
        
        public DeleteBookCommandHandler(IBookRepository bookRepository)
        {
           
            _bookRepository = bookRepository;
           
        }
        public async Task<string> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetBookNoTracking(request.bookId);
            if (book == null) return "Book does not exist";

            _bookRepository.DeleteBook(book);
            return "Successfully deleted";
        }
    }
}
