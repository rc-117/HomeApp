namespace Homeapp.Backend.RequestHandling
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (HttpResponseException e)
            {
                context.Response.StatusCode = (int)e.Response.StatusCode;
                var content = await e.Response.Content.ReadAsStringAsync();
                await context.Response.WriteAsync(
                    string.Format("{0}: {1}",
                        e.Response.ReasonPhrase,
                        content));
            }
        }
    }
}