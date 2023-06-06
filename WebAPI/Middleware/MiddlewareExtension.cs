namespace WebAPI.Middleware
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseCustomExceptions(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
