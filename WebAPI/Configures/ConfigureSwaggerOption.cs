using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace WebAPI.Configures
{
    public class ConfigureSwaggerOption : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        public ConfigureSwaggerOption(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                var apiVersion = description.ApiVersion.ToString();
                options.SwaggerDoc(description.GroupName,
                    new OpenApiInfo
                    {
                        Version = apiVersion,
                        Title = $"TreeNotes API {apiVersion}",
                        Description = "Web API for TreeNotes application based on ASP.NET Core WebAPI",
                        Contact = new OpenApiContact
                        {
                            Name = "David Manvel",
                            Email = "10kwelldavid@gmail.com",
                            Url = new Uri("https://github.com/manveldavid")
                        }
                    });
                options.CustomOperationIds(apiDescription =>
                apiDescription.TryGetMethodInfo(out MethodInfo methodInfo) ?
                methodInfo.Name : null);
            }
        }
    }
}
