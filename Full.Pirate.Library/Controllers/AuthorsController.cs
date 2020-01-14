
using AutoMapper;
using Full.Pirate.Library.Models;
using Full.Pirate.Library.Services;
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


        public ActionResult<IEnumerable<AuthorDto>> Actors()
        {
            var authors = service.GetAuthors();
            var authorsDto = mapper.Map<IEnumerable<AuthorDto>>(authors);
            return Ok(authorsDto);
        }
    }
}
