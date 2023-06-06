using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.EntityTypeConfiguration;

namespace Persistence.Contexts
{
    public class UsersDbContext : DbContext, IDbContext<TreeNoteUser>
    {
        public DbSet<TreeNoteUser> Set { get; set; }
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserDbConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
