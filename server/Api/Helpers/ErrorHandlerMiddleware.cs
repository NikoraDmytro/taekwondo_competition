using System.Data;
using System.Net;
using System.Text.Json;
using Core.Exceptions;

namespace Api.Helpers
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var message = error.Message;
                var response = context.Response;
                response.ContentType = "application/json";
                
                switch(error)
                {
                    case AppException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case DuplicateNameException e:
                        response.StatusCode = (int) HttpStatusCode.BadRequest;
                        break;
                    default:
                        Console.WriteLine(error);
                        message = "Internal Server Error";
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new
                {
                    statusCode = response.StatusCode,
                    message
                });
                
                await response.WriteAsync(result);
            }
        }
    }
}