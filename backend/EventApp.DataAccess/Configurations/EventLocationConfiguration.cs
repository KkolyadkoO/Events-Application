using EventApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventApp.DataAccess.Configurations;

public class EventLocationConfiguration : IEntityTypeConfiguration<LocationOfEvent>
{
    public void Configure(EntityTypeBuilder<LocationOfEvent> builder)
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