using DevTalk.Customers.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DevTalk.Customers.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case BadRequestException _:
                        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case NotFoundException _:
                        httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                await httpContext.Response.WriteAsync(ex.Message);
            }
        }
    }
}
