
using Microsoft.AspNetCore.Diagnostics;
using MotorcycleRental.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace MotorcycleRental.API.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, message) = exception switch
            {
                NotFoundException => (HttpStatusCode.NotFound, exception.Message),
                DomainException => (HttpStatusCode.BadRequest, exception.Message),
                _ => (HttpStatusCode.InternalServerError, "Ocorreu um erro interno no servidor.")
            };

            httpContext.Response.StatusCode = (int)statusCode;
            httpContext.Response.ContentType = "application/json";

            var problemDetails = new
            {
                mensagem = message
            };

            var jsonResponse = JsonSerializer.Serialize(problemDetails);
            await httpContext.Response.WriteAsync(jsonResponse, cancellationToken);

            return true;
        }
    }
}
