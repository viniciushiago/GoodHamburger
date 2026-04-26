using FluentValidation;
using GoodHamburger.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace GoodHamburger.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                await HandleValidationException(context, ex);
            }
            catch (NegocioException ex)
            {
                await HandleNegocioException(context, ex);
            }
            catch (NaoEncontradoException ex)
            {
                await HandleNaoEncontradoException(context, ex);
            }
            catch (Exception ex)
            {
                await HandleGenericException(context, ex);
            }
        }

        private static async Task HandleValidationException(HttpContext context, ValidationException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var erros = ex.Errors.Select(x => new
            {
                campo = x.PropertyName,
                mensagem = x.ErrorMessage
            });

            await context.Response.WriteAsync(JsonSerializer.Serialize(new { erros }));
        }

        private static async Task HandleNegocioException(HttpContext context, NegocioException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                mensagem = ex.Message
            }));
        }

        private static async Task HandleNaoEncontradoException(HttpContext context, NaoEncontradoException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                mensagem = ex.Message
            }));
        }

        private static async Task HandleGenericException(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                mensagem = "Ocorreu um erro inesperado."
            }));
        }
    }
}