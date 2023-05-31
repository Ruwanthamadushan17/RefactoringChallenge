using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace RefactoringChallenge.Common
{
    public static class GlobalExceptionHandler
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app, IHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.Use(DevResponse);
            }
            else
            {
                app.Use(ProdResponse);
            }
        }

        private static Task DevResponse(HttpContext httpContext, Func<Task> next)
        => WriteResponse(httpContext, includeDetails: true);

        private static Task ProdResponse(HttpContext httpContext, Func<Task> next)
            => WriteResponse(httpContext, includeDetails: false);

        private static async Task WriteResponse(HttpContext httpContext, bool includeDetails)
        {
            var exceptionDetails = httpContext.Features.Get<IExceptionHandlerFeature>();
            var ex = exceptionDetails?.Error;

            httpContext.Response.ContentType = "application/problem+json";

            var title = includeDetails ? "An error occured: " + ex.Message : "An error occured";
            var details = includeDetails ? ex.ToString() : null;
            

            var problem = new ProblemDetails
            {
                Status = 500,
                Title = title,
                Detail = details
            };

            var stream = httpContext.Response.Body;
            await JsonSerializer.SerializeAsync(stream, problem);
        }
    }
}
