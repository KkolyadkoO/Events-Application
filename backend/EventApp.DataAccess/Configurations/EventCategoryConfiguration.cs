using EventApp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventApp.DataAccess.Configurations;

public class EventCategoryConfiguration : IEntityTypeConfiguration<CategoryOfEventEntity>
{
    public void Configure(EntityTypeBuilder<CategoryOfEventEntity> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder
            .HasIndex(c => c.Title)
            .IsUnique();

        builder
            .Property(c => c.Title)
            .IsRequired();
    }
}