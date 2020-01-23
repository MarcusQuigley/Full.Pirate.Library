
using AutoMapper;
using Full.Pirate.Library.Entities;
using Full.Pirate.Library.Helpers;
using Full.Pirate.Library.Models;
using Full.Pirate.Library.SearchParams;
using Full.Pirate.Library.Services;
using Full.Pirate.Library.Services.Sorting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
 
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Full.Pirate.Library.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        readonly IRepositoryService service;
        readonly IPropertyMappingService propertyMappingService;
        readonly IMapper mapper;
        readonly IDataShapeValidator dataShapeValidator;

        public AuthorsController(IRepositoryService service,
            IPropertyMappingService propertyMappingService,
            IMapper mapper,
            IDataShapeValidator dataShapeValidator)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.dataShapeValidator = dataShapeValidator ?? throw new ArgumentNullException(nameof(dataShapeValidator));
        }

        [HttpGet(Name = "GetAuthors")]
        [HttpHead]
        public IActionResult GetAuthors(
            [FromQuery]  AuthorsResourceParameters authorParms)
        {
            if (!propertyMappingService.ValidMappingExistsFor<Models.AuthorDto, Author>
                (authorParms.OrderBy))
            {
                return BadRequest();
            }
            if (!dataShapeValidator.CheckFieldsExist<Author>(authorParms.Fields))
            {
                return BadRequest();
            }
            var authors =  service.GetAuthors(authorParms);

            this.Response.Headers.Add("X-Pagination", CreatePaginationHeader(authors, authorParms));
            return Ok(mapper.Map<IEnumerable<AuthorDto>>(authors).ShapeData(authorParms.Fields));
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
        public ActionResult<AuthorDto> CreateAuthor([FromBody] AuthorToCreateDto authorToCreate)
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

        [HttpPatch("{authorId}")]
        public ActionResult PatchAuthor(Guid authorId, JsonPatchDocument<AuthorToCreateDto> authorClient)
        {
            var author = service.GetAuthor(authorId);
            if (author == null)
            {
                return NotFound();
            }
            var authorToPatch = mapper.Map<AuthorToCreateDto>(author);

            authorClient.ApplyTo(authorToPatch, ModelState);
 
            if (!TryValidateModel(authorToPatch))
            {
                return ValidationProblem(this.ModelState);
            }
               author = mapper.Map<Author>(authorToPatch);
            service.UpdateAuthor(author);
            if (service.Save())
            {
                return NoContent();
            }
            return BadRequest();
         }
       
        [HttpDelete("{authorId}")]
        public ActionResult DeleteAuthor(Guid authorId)
        {
            var author = service.GetAuthor(authorId);
            if (author==null)
            {
                return NotFound();
            }
            service.DeleteAuthor(author);
            if (service.Save())
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpOptions]
        public ActionResult<string> GetAuthorOptions()
        {
            Response.Headers.Add("Allow","DELETE, GET, HEAD, OPTIONS, POST, PATCH");
            return Ok();
        }

        private string CreatePaginationHeader(PagedList<Author> authors, AuthorsResourceParameters authorParms)
        {
            var previousPageLink = authors.HasPrevious ?
                (CreateAuthorsResourceUri(authorParms, ResourceUriType.PreviousPage)) : null;
            var nextPageLinkLink = authors.HasNext ?
                (CreateAuthorsResourceUri(authorParms, ResourceUriType.NextPage)) : null;
            var paginationMetadata = new
            {
                Fields = authorParms.Fields,
                totalCount = authors.TotalCount,
                pageSize = authors.PageSize,
                CurrentPage = authors.CurrentPage,
                TotalPages = authors.TotalPages,
                previousPageLink,
                nextPageLinkLink
            };

            return JsonSerializer.Serialize(paginationMetadata);
        }

        private string CreateAuthorsResourceUri(AuthorsResourceParameters authorsParams, ResourceUriType type)
        {
            int pageNumber = authorsParams.PageNumber;
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    pageNumber -= 1;
                    break;
                case ResourceUriType.NextPage:
                    pageNumber += 1;
                    break;
                default:
                    break;
            }
            return Url.Link("GetAuthors",
                new
                {
                    pageNumber = pageNumber,
                    pageSize = authorsParams.PageSize,
                    mainCategory = authorsParams.MainCategory,
                    searchQuery = authorsParams.SearchQuery
                });
        }
 
    }
}
