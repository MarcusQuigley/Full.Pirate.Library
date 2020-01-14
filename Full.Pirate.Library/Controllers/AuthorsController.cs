using Full.Pirate.Library.Entities;
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

        public AuthorsController(IRepositoryService service)
        {
            this.service = service;
        }


        public ActionResult<IEnumerable<Author>> Actors()
        {
            var authors = service.GetAuthors();
            return Ok(authors);
        }
    }
}
