using AutoMapper;
using Full.Pirate.Library.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Full.Pirate.Library.Filters
{
    public class AuthorsResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var resultsFromAction = context.Result as ObjectResult;
            if (resultsFromAction?.Value == null ||
                resultsFromAction.StatusCode < 200 ||
                resultsFromAction.StatusCode >= 300)
            {
                await next();
                return;
            }
            var mapper = context.HttpContext.RequestServices.GetService(typeof(IMapper)) as IMapper;
            resultsFromAction.Value = mapper.Map<IEnumerable<Author>>(resultsFromAction.Value);
            await next();
        }
    }
}
