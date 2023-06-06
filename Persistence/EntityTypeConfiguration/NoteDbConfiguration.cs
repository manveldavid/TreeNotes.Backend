using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityTypeConfiguration
{
    public class NoteDbConfiguration:IEntityTypeConfiguration<TreeNote>
    {
        public void Configure(EntityTypeBuilder<TreeNote> builder) 
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}
