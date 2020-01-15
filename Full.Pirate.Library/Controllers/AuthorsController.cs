
using AutoMapper;
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

        [HttpGet("{authorId}")]
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
    }
}
