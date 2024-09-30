using EventApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventApp.DataAccess.Configurations;

public class EventCategoryConfiguration : IEntityTypeConfiguration<CategoryOfEvent>
{
    public void Configure(EntityTypeBuilder<CategoryOfEvent> builder)
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