using EventApp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventApp.DataAccess.Configurations;

public class MemberOfEventConfiguration : IEntityTypeConfiguration<MemberOfEventEntity>
{
    public void Configure(EntityTypeBuilder<MemberOfEventEntity> builder)
    {
        builder.HasKey(a => a.Id);
    }
}
