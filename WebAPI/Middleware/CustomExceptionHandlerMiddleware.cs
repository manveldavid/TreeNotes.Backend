using Application.Common.Exceptions;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace WebAPI.Middleware
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleCustomException(context, ex);
            }
        }
        private Task HandleCustomException(HttpContext context, Exception ex)
        {
            var responseCode = HttpStatusCode.InternalServerError;
            var result = JsonSerializer.Serialize("unknown error");

            switch (ex)
            {
                case ValidationException validationException:
                    responseCode = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(validationException.Errors);
                    break;
                case EntityNotFoundException noteNotFoundException:
                    responseCode = HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(noteNotFoundException.Message);
                    break;
                case EntityDeniedPermissionException deniedPermissionException:
                    responseCode = HttpStatusCode.Forbidden;
                    result = JsonSerializer.Serialize(deniedPermissionException.Message);
                    break;
                case TreeNoteParentCheckException treeNoteParentCheckException:
                    responseCode = HttpStatusCode.Locked;
                    result = JsonSerializer.Serialize(treeNoteParentCheckException.Message);
                    break;
                case TreeNoteUserExistException treeNoteUserExistException:
                    responseCode = HttpStatusCode.Conflict;
                    result = JsonSerializer.Serialize(treeNoteUserExistException.Message);
                    break;
                case TreeNoteUserWrongPasswordException treeNoteUserWrongPasswordException:
                    responseCode = HttpStatusCode.Forbidden;
                    result = JsonSerializer.Serialize(treeNoteUserWrongPasswordException.Message);
                    break;
                case InvalidRequestException invalidRequestException:
                    responseCode = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(invalidRequestException.Message);
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)responseCode;

            if (result == string.Empty)
            {
                result = JsonSerializer.Serialize(new { error = ex.Message });
            }

            return context.Response.WriteAsync(result);
        }
    }
}
