using System.Net;
using System.Threading.Tasks;
using SwaggerGlobalization.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace SwaggerGlobalization.Infrastructure.Middlewares
{
    public class BaseMiddleware
    {
        private readonly RequestDelegate _next;

        public BaseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IConfiguration configuration)
        {

            //enable request body access
            context.Request.EnableBuffering();
         
            await _next(context);


            //manage error status
            if (context.Response.StatusCode >= StatusCodes.Status400BadRequest)
            {
                context.Response.ContentType = "application/json";

               
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new BaseResponse
                {
                    Error = new Error()
                    {
                        ErrorCode = context.Response.StatusCode,
                        ErrorMessage = ((HttpStatusCode)context.Response.StatusCode).ToString()
                    },
                    RequestStatus = RequestStatus.KO.ToString()
                }
                ));
            }
        }

    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class BaseMiddlewareExtensions
    {
        public static IApplicationBuilder UseBaseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BaseMiddleware>();
        }
    }
}
