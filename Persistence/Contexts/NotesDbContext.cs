using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.EntityTypeConfiguration;

namespace Persistence.Contexts
{
    public class NotesDbContext : DbContext, IDbContext<TreeNote>
    {
        public DbSet<TreeNote> Set { get; set; }
        public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new NoteDbConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
