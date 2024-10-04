using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApp.DataAccess.Repositories;

public class MembersOfEventRepository : Repository<MemberOfEvent>, IMembersOfEventRepository
{
    public MembersOfEventRepository(EventAppDBContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<MemberOfEvent>> GetByEventIdAsync(Guid eventId)
    {
        return await _dbContext.Set<MemberOfEvent>()
            .AsNoTracking()
            .Where(m => m.EventId == eventId)
            .ToListAsync();
    }

    public async Task<List<MemberOfEvent>> GetByUserIdAsync(Guid userId)
    {
        return await _dbContext.Set<MemberOfEvent>()
            .AsNoTracking()
            .Where(m => m.UserId == userId)
            .ToListAsync();
    }

    public async Task<MemberOfEvent> GetByEventIdAndUserIdAsync(Guid eventId, Guid userId)
    {
        return await _dbContext.Set<MemberOfEvent>()
            .FirstOrDefaultAsync(m => m.EventId == eventId && m.UserId == userId);
    }

    public async Task DeleteByEventIdAndUserIdAsync(Guid eventId, Guid userId)
    {
        var member = await _dbContext.Set<MemberOfEvent>()
            .FirstOrDefaultAsync(m => m.EventId == eventId && m.UserId == userId);
        _dbContext.Set<MemberOfEvent>().Remove(member);
    }
}