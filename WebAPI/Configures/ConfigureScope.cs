using Persistence.Common;
using Persistence.Contexts;
using Serilog;

namespace WebAPI.Configures
{
    public static class ConfigureScope
    {
        public static void Configure(IServiceScope provider)
        {
            using (var scope = provider)
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var notesContext = serviceProvider.GetRequiredService<NotesDbContext>();
                    var usersContext = serviceProvider.GetRequiredService<UsersDbContext>();
                    DbInitializer.Initialize(notesContext);
                    DbInitializer.Initialize(usersContext);
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "An error occured while DB initialization");
                }
            }
        }
    }
}
