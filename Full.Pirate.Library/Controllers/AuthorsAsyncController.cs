//#undef TESTING_BANDWIDTH
using AutoMapper;
using Full.Pirate.Library.Entities;
using Full.Pirate.Library.Filters;
using Full.Pirate.Library.Helpers;
using Full.Pirate.Library.Models;
using Full.Pirate.Library.SearchParams;
using Full.Pirate.Library.Services;
using Full.Pirate.Library.Services.Sorting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Full.Pirate.Library.Controllers
{
    [Route("api/authorsasync")]
    [ApiController]
    public class AuthorsAsyncController :ControllerBase
    {
        readonly IRepositoryService service;
        readonly IPropertyMappingService propertyMappingService;
        readonly IMapper mapper;
        readonly IDataShapeValidatorService dataShapeValidatorService;

        public AuthorsAsyncController(IRepositoryService service,
            IPropertyMappingService propertyMappingService,
            IMapper mapper,
            IDataShapeValidatorService dataShapeValidator)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.dataShapeValidatorService = dataShapeValidator ?? throw new ArgumentNullException(nameof(dataShapeValidator));
        }

#if (!TESTING_BANDWIDTH)
        [HttpGet(Name = "GetAuthorsAsync")]
        [HttpHead]
        public async Task<IActionResult> GetAuthorsAsync(
            [FromQuery]  AuthorsResourceParameters authorParms)
        {
            if (!propertyMappingService.ValidMappingExistsFor<Models.AuthorDto, Author>
                (authorParms.OrderBy))
            {
                return BadRequest();
            }
            if (!dataShapeValidatorService.CheckFieldsExist<AuthorDto>(authorParms.Fields))
            {
                return BadRequest();
            }
            var authors = await service.GetAuthorsAsync(authorParms);

            this.Response.Headers.Add("X-Pagination", CreatePaginationHeader(authors, authorParms));
            return Ok(mapper.Map<IEnumerable<AuthorDto>>(authors).ShapeData(authorParms.Fields));
        }
#endif
#if (TESTING_BANDWIDTH)
        [HttpGet]
        [AuthorsResultFilter]
        public async Task<IActionResult> GetAuthorsAsync()
        {
            var authors = await service.GetAuthorsAsync();

            return Ok(authors);
        }
#endif
        [HttpGet("{authorId}", Name = "GetAuthorAsync")]
        [HttpHead("{authorId}")]
        public async Task<ActionResult<AuthorDto>> GetAuthorAsync(Guid authorId, string fields)
        {
            if (!dataShapeValidatorService.CheckFieldsExist<AuthorDto>(fields))
            {
                return BadRequest();
            }
            var author = await service.GetAuthorAsync(authorId);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<AuthorDto>(author).ShapeData(fields));
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

