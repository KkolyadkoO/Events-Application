using EventApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventApp.DataAccess.Configurations;

public class MemberOfEventConfiguration : IEntityTypeConfiguration<MemberOfEvent>
{
    public void Configure(EntityTypeBuilder<MemberOfEvent> builder)
    {
        builder.HasKey(a => a.Id);
    }
}
