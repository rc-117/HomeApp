namespace Homeapp.Backend.RequestHandling
{
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json.Linq;
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

                    new JObject()
                    {
                        { "StatusCode", (int)e.Response.StatusCode },
                        { "Error", e.Response.ReasonPhrase },
                        { "Message", content }
                    }.ToString());
            }
        }
    }
}