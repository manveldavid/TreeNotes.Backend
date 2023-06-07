using Microsoft.AspNetCore.Mvc.ApiExplorer;
using WebAPI.Middleware;

namespace WebAPI.Configures
{
    public static class ConfigurePipeLine
    {
        public static void Configure(WebApplication app)
        {
            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                var apiVersionDescriptions =
                    app.Services.GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions;
                foreach (var description in apiVersionDescriptions)
                {

                    config.SwaggerEndpoint(
                        $"swagger/{description.GroupName}/swagger.json",
                        $"{description.GroupName.ToUpperInvariant()}");
                }
                config.RoutePrefix = string.Empty;
            });
            app.UseCustomExceptions();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseApiVersioning();
            app.MapControllers();
        }
    }
}
