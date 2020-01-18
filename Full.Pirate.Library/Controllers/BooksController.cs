using AutoMapper;
using Full.Pirate.Library.Entities;
using Full.Pirate.Library.Models;
using Full.Pirate.Library.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Full.Pirate.Library.Controllers
{
    [ApiController]
    [Route("api/authors/{authorId}/books")]
    public class BooksController : ControllerBase
    {
        readonly IRepositoryService service;
        readonly IMapper mapper;
        public BooksController(IRepositoryService service, IMapper mapper)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        [HttpGet("{bookId}", Name ="GetBook")]
        public ActionResult<BookDto> GetBookForAuthor(Guid authorId,  Guid bookId)
        {
            if (authorId == Guid.Empty || bookId == Guid.Empty)
            {
                return BadRequest();
            }

            if (!service.AuthorExists(authorId))
            {
                return BadRequest();
            }
                        
            var book = service.GetBook(authorId, bookId);
            if (book == null)
            {
                return NotFound();
            }

            var bookDto = mapper.Map<BookDto>(book);
            return Ok(bookDto);
        }

        [HttpGet]
        public ActionResult<IEnumerable<BookDto>> GetBooksForAuthor(Guid authorId)
        {
            if (!service.AuthorExists(authorId))
            {
                return BadRequest();
            }
            var bookEntities = service.GetBooks(authorId);
            var bookDtos = mapper.Map<IEnumerable<BookDto>>(bookEntities);
            return Ok(bookEntities);
        }

        [HttpPost]
        public ActionResult<BookDto> AddBook(Guid authorId, BookToCreateDto bookToCreate)
        {
            if (authorId == Guid.Empty)
            {
                return BadRequest();
            }
            if (bookToCreate == null)
            {
                return BadRequest();
            }
            if (!service.AuthorExists(authorId))
            {
                return NotFound();
            }
            //bookToCreate.AuthorId = authorId;
            var bookEntity = mapper.Map<Book>(bookToCreate);
            service.AddBook(authorId, bookEntity);
            if (service.Save())
            {
                var bookDto = mapper.Map<BookDto>(bookEntity);
                return CreatedAtRoute("GetBook", new { authorId = authorId, bookId = bookDto.Id }, bookDto);
            }
            return BadRequest(); // InternalServerErrorResult()
        }

        [HttpDelete("{bookId}")]
        public ActionResult DeleteBook(Guid authorId, Guid bookId)
        {
            
            if (!service.AuthorExists(authorId))
            {
                return NotFound();
            }
            var book = service.GetBook(authorId, bookId);
            if (book == null)
            {
                return NotFound();
            }
            service.DeleteBook(book);
            if (service.Save())
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpOptions]
        public ActionResult GetOptions()
        {
            this.Response.Headers.Add("Action", "GET, DELETE, POST");
            return Ok();
        }
    }
}
