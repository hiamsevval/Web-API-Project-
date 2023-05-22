using BookStore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using BookStore.DTO;
using BookStore.Queries;
using MediatR;
using BookStore.Commands;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        
        private readonly IMediator _mediator;

        public BookController( IMediator mediator)
        {
            
            _mediator = mediator;
            
        }

        [HttpGet("BookSearch")]///{str}
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDto>))]
        public async Task<IActionResult> GetBooks(string str)

        {
            var query = new GetBooksQuery(str);
            var result = await _mediator.Send(query);
            return result != null ? (IActionResult)Ok(result) : NotFound();
            
        }

        [HttpGet("{bookId}")]
        [ProducesResponseType(200, Type = typeof(Book))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetBook(int bookId)
        {
            
            var query = new GetBookQuery(bookId);
            var result = await _mediator.Send(query);
            return result != null ? (IActionResult) Ok(result) : NotFound();
            
        }
       
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateBook([FromBody] BookDto request)

        {
            BookDto book=null;
            if (request != null)
            {
                book = await _mediator.Send(new CreateBookCommand(

                    request.Name,
                    request.Author,
                    request.PublishYear));
            }
            else return BadRequest();
            

            return Ok(book);
        }

        [HttpPut("{bookId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateBook(int bookId, [FromBody] BookDto updatedBook)
        {
            if (bookId != updatedBook.Id) return BadRequest();

            var bookUpdated = await _mediator.Send(new UpdateBookCommand(

                updatedBook.Id,
                updatedBook.Name,
                updatedBook.Author,
                updatedBook.PublishYear
                ));


            return Ok(bookUpdated);
        }
        
        [HttpDelete("{bookId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            var command = new DeleteBookCommand(bookId);
            var result = await _mediator.Send(command);
            if (result == "Successfully deleted") return Ok(result);
            else return BadRequest();
        }

    }
}
