using EventApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventApp.DataAccess.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(a => a.Id);

        builder
            .HasMany(a => a.Members)
            .WithOne()
            .HasForeignKey(m => m.EventId)
            .OnDelete(DeleteBehavior.Cascade); 
        builder
            .HasOne<CategoryOfEvent>()
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne<LocationOfEvent>()
            .WithMany()
            .HasForeignKey(e => e.LocationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}