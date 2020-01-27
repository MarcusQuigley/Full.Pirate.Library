using AutoMapper;
using Full.Pirate.Library.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Full.Pirate.Library.Filters
{
    public class AuthorResultFilterAttribute : ResultFilterAttribute
    {
         public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var resultFromAction = context.Result as ObjectResult;
            if (resultFromAction?.Value == null ||
                resultFromAction.StatusCode < 200 ||
                resultFromAction.StatusCode >=300)
            {
                await next();
                return;
            }
            var mapper = context.HttpContext.RequestServices.GetService(typeof(IMapper)) as IMapper;

            resultFromAction.Value = mapper.Map<Author>(resultFromAction.Value);
            await next();
        }
    }
}
