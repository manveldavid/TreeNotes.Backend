using Microsoft.EntityFrameworkCore;

namespace Persistence.Common
{
    public static class DbInitializer
    {
        public static void Initialize(DbContext context) 
        {
            context.Database.EnsureCreated();
        }
    }
}
