using E_Commerece.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace E_Commerece.API.Attributes
{
    public class RedisCachedAttribute : ActionFilterAttribute
    {

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var cachService = context.HttpContext.RequestServices.GetRequiredService<ICachService>();
            var cachkey = CreateCacheKeyFromRequest(context.HttpContext.Request);
            var cached = await cachService.GetAsync(cachkey);

            if (!string.IsNullOrEmpty(cached))
            {
                context.Result = new ContentResult()
                {
                    Content = cached,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            var execute = await next.Invoke();
            if (execute.Result is OkObjectResult { Value: not null } ok)
            {
                await cachService.SetAsync(cachkey, ok.Value, TimeSpan.FromSeconds(1000));
            }
            return;

        }


        public static string CreateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append(request.Path).Append("?");

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append(key).Append("=").Append(value).Append("&");
            }
            return keyBuilder.ToString();

        }
    }
}
