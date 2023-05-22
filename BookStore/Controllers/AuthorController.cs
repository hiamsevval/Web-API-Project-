using AutoMapper;
using BookStore.Commands;
using BookStore.DTO;
using BookStore.Interfaces;
using BookStore.Models;
using BookStore.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : Controller
    {

        private readonly IMediator _mediator;
     

        public AuthorController(IMediator mediator)
        {
            _mediator = mediator;
           
        }

        [HttpGet("authorSearch")]///{str}
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDto>))]
        public async Task<IActionResult> GetAuthors(string str)

        {
            var query = new GetAuthorsQuery(str);
            var result = await _mediator.Send(query);
            
            return result != null ? (IActionResult)Ok(result) : NotFound();

        }

        [HttpGet("{authorId}")]
        [ProducesResponseType(200, Type = typeof(Author))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAuthor(int authorId)
        {
            var query = new GetAuthorQuery(authorId);
            var result = await _mediator.Send(query); 
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorDto request)
        {
            Author author=null;

            if (request != null)
            {
                author = await _mediator.Send(new CreateAuthorCommand(

                request.Name,
                request.Surname));
            }
            
           
            return author != null ? (IActionResult)Ok(author) : BadRequest();
        }

        [HttpPut("{authorId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateAuthor(int authorId, [FromBody]AuthorDto updatedAuthor)
        {
            if (authorId != updatedAuthor.Id) return BadRequest();

            var authorUpdated = await _mediator.Send(new UpdateAuthorCommand(

                updatedAuthor.Id,
                updatedAuthor.Name,
                updatedAuthor.Surname
                ));
            
            
            return Ok(authorUpdated);
        }
        
        [HttpDelete("{authorId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAuthor(int authorId)
        {
            var command = new DeleteAuthorCommand(authorId);
            var result = await _mediator.Send(command);
            if (result == "Successfully deleted")
                return Ok(result);
            else return BadRequest();
        }
        
    }
}
