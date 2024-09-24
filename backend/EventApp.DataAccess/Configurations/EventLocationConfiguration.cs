using EventApp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventApp.DataAccess.Configurations;

public class EventLocationConfiguration : IEntityTypeConfiguration<LocationOfEventEntity>
{
    public void Configure(EntityTypeBuilder<LocationOfEventEntity> builder)
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