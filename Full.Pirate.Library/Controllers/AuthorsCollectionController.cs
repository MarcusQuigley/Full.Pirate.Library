using AutoMapper;
using Full.Pirate.Library.Helpers;
using Full.Pirate.Library.Models;
using Full.Pirate.Library.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Full.Pirate.Library.Controllers
{
    [ApiController]
    [Route("api/authorscollection")]
    public class AuthorsCollectionController : ControllerBase
    {
        readonly IRepositoryService service;
        readonly IMapper mapper;

        public AuthorsCollectionController(IRepositoryService service, IMapper mapper)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("({ids})", Name = "GetAuthors")]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthorCollection(
            [FromRoute]
            [ModelBinder(typeof(ArrayModelBinding))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }
            var authorsEntities = service.GetAuthors(ids);
            if (ids.Count() != authorsEntities.Count())
            {
                return NotFound();
            }

            var result = mapper.Map<IEnumerable<AuthorDto>>(authorsEntities);
 
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<IEnumerable<AuthorDto>> CreateAuthors(
            IEnumerable<AuthorToCreateDto> authors)
        {
            if (authors == null)
            {
                return BadRequest();
            }
            //var authorDtoCreatedList = new List<AuthorDto>();
            //var routeList = new List<CreatedAtRouteResult>();
            var authorEntities = mapper.Map<IEnumerable<Entities.Author>>(authors);
            foreach (var authorToCreate in authorEntities)
            {
                service.AddAuthor(authorToCreate);
            }
            service.Save();

            var authorsDtos = mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
            //if (service.Save())
            //{
            //    var authorDto = mapper.Map<AuthorDto>(author);
            //    authorDtoCreatedList.Add(authorDto);
            //    routeList.Add(new CreatedAtRouteResult("GetAuthors", new { authorId = authorDto.AuthorId }, authorDto));
            //}
            //}

            var idsAsString = string.Join(",", authorsDtos.Select(aDto => aDto.AuthorId));

            return CreatedAtRoute(
                "GetAuthors",
                new
                {
                    ids = idsAsString
                },
                authorsDtos);
            //return Ok( );
        }
    }
}
