
using AutoMapper;
using Full.Pirate.Library.Entities;
using Full.Pirate.Library.Models;
using Full.Pirate.Library.SearchParams;
using Full.Pirate.Library.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Full.Pirate.Library.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        readonly IRepositoryService service;
        readonly IMapper mapper;

        public AuthorsController(IRepositoryService service,
            IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }
        
        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors([FromQuery]  AuthorsResourceParameters authorParms)
        {
            var authors = service.GetAuthors(authorParms);
            var authorsDto = mapper.Map<IEnumerable<AuthorDto>>(authors);
            return Ok(authorsDto);
        }

        [HttpGet("{authorId}",Name ="GetAuthor")]
        [HttpHead("{authorId}")]
        public ActionResult<AuthorDto> GetAuthor(Guid authorId)
        {
            var author = service.GetAuthor(authorId);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<AuthorDto>(author));

        }

        [HttpPost]
        public ActionResult<AuthorDto> CreateAuthor(AuthorToCreateDto authorToCreate)
        {
            var authorEntity = mapper.Map<Author>(authorToCreate);
            service.AddAuthor(authorEntity);
            if (service.Save())
            {
                var authorDto = mapper.Map<AuthorDto>(authorEntity);
 
                  return CreatedAtRoute("GetAuthor",new { authorId = authorDto.AuthorId }, authorDto);
            }
            return BadRequest();


        }

        [HttpGet]
        [Route("unmapped")]
        public ActionResult<IEnumerable<Author>> GetAuthorsAsEntities()
        {
            var authors = service.GetAuthors();
            if (authors!=null)
            {
                if (authors.Count() > 1)
                {
                    return Ok(authors.Take(2));
                }
                else {
                    return Ok(authors);
                }
            }
            return BadRequest();
            
        }
    }
}
