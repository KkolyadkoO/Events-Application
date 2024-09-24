using EventApp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventApp.DataAccess.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasIndex(u => u.UserEmail)
            .IsUnique();
        builder.HasIndex(u => u.UserName)
            .IsUnique();
        builder
            .HasMany(a => a.MemberOfEvents)
            .WithOne()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}
