using EventApp.DataAccess.Configurations;
using EventApp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventApp.DataAccess;

public class EventAppDBContext(DbContextOptions<EventAppDBContext> options) : DbContext(options)
{
    public DbSet<EventEntity> EventEntities { get; set; }
    public DbSet<MemberOfEventEntity> MemberOfEventEntities { get; set; }
    public DbSet<CategoryOfEventEntity> CategoryOfEventEntities { get; set; }
    public DbSet<UserEntity> UserEntities { get; set; }
    
    public DbSet<RefreshTokenEntity> RefreshTokenEntities { get; set; }
    
    public DbSet<LocationOfEventEntity> LocationsOfEventEntities { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EventConfiguration());
        modelBuilder.ApplyConfiguration(new EventCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new MemberOfEventConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
