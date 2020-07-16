using SwaggerGlobalization.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace SwaggerGlobalization.Infrastructure.Middlewares
{
    public static class GlobalExceptionMiddleware
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {

                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {

                       
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new BaseResponse
                        {
                            Error = new Error()
                            {
                                ErrorCode = (int)HttpStatusCode.BadRequest,
                                ErrorMessage = contextFeature.Error.Message
                            },
                            RequestStatus = RequestStatus.KO.ToString()
                        }
                        ));
                    }
                });
            });
        }
    }
}
