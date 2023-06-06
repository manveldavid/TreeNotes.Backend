using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityTypeConfiguration
{
    public class UserDbConfiguration : IEntityTypeConfiguration<TreeNoteUser>
    {
        public void Configure(EntityTypeBuilder<TreeNoteUser> builder) 
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();
            builder.HasIndex(x => x.Login).IsUnique();
        }
    }
}
