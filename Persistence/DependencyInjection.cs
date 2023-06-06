using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, 
            IConfiguration configuration)
        {
            var notesConnectionString = configuration["NotesDbConnection"];
            var usersConnectionString = configuration["UsersDbConnection"];

            services.AddDbContext<NotesDbContext>(options =>
            {
                options.UseSqlite(notesConnectionString);
            });
            services.AddDbContext<UsersDbContext>(options =>
            {
                options.UseSqlite(usersConnectionString);
            });

            services.AddScoped<IDbContext<TreeNote>>(provider =>
                provider.GetService<NotesDbContext>());
            services.AddScoped<IDbContext<TreeNoteUser>>(provider =>
                provider.GetService<UsersDbContext>());

            return services;
        }
    }
}
