using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Application;
using Persistence;
using Microsoft.AspNetCore.Authentication.Certificate;

namespace WebAPI.Configures
{
    public static class ConfigureServices
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();
            services.AddPersistance(configuration);
            services.AddControllers();
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                options.ReportApiVersions = true;
            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOption>();
            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate();
        }
    }
}
