using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface IDbContext<Entity> where Entity : class
    {
        DbSet<Entity> Set { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
