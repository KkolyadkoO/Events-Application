using EventApp.Core.Models;
using EventApp.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;

namespace EventApp.DataAccess;

public class EventAppDBContext(DbContextOptions<EventAppDBContext> options) : DbContext(options)
{
    public DbSet<Event> EventEntities { get; set; }
    public DbSet<MemberOfEvent> MemberOfEventEntities { get; set; }
    public DbSet<CategoryOfEvent> CategoryOfEventEntities { get; set; }
    public DbSet<User> UserEntities { get; set; }
    
    public DbSet<RefreshToken> RefreshTokenEntities { get; set; }
    
    public DbSet<LocationOfEvent> LocationsOfEventEntities { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EventConfiguration());
        modelBuilder.ApplyConfiguration(new EventCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new EventLocationConfiguration());
        modelBuilder.ApplyConfiguration(new MemberOfEventConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
