using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApp.DataAccess.Repositories;

public class MembersOfEventRepository : IMembersOfEventRepository
{
    private readonly EventAppDBContext _dbContext;

    public MembersOfEventRepository(EventAppDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<MemberOfEvent>> Get()
    {
        var memberOfEvents = await _dbContext.MemberOfEventEntities
            .AsNoTracking()
            .ToListAsync();
        return memberOfEvents;
    }

    public async Task<MemberOfEvent> GetById(Guid id)
    {
        var memberOfEvent = await _dbContext.MemberOfEventEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
        return memberOfEvent;
    }

    public async Task<List<MemberOfEvent>> GetByEventId(Guid eventId)
    {
        var members = await _dbContext.MemberOfEventEntities.AsNoTracking()
            .Where(m => m.EventId == eventId)
            .ToListAsync();
        return members;
    }

    public async Task<List<MemberOfEvent>> GetByUserId(Guid userId)
    {
        var members = await _dbContext.MemberOfEventEntities.AsNoTracking()
            .Where(m => m.UserId == userId)
            .ToListAsync();
        return members;
    }

    public async Task<Guid> Create(MemberOfEvent memberOfEvent)
    {
        await _dbContext.MemberOfEventEntities.AddAsync(memberOfEvent);
        return memberOfEvent.Id;
    }


    public async Task<bool> Update(MemberOfEvent memberOfEvent)
    {
        var foundedMember = await _dbContext.MemberOfEventEntities
            .Where(m => m.Id == memberOfEvent.Id)
            .FirstOrDefaultAsync();
        if (foundedMember == null)
        {
            return false;
        }

        foundedMember.Name = foundedMember.Name;
        foundedMember.LastName = foundedMember.LastName;
        foundedMember.Birthday = foundedMember.Birthday;
        foundedMember.DateOfRegistration = foundedMember.DateOfRegistration;
        foundedMember.Email = foundedMember.Email;
        foundedMember.UserId = foundedMember.UserId;
        foundedMember.EventId = foundedMember.EventId;

        _dbContext.MemberOfEventEntities.Update(foundedMember);
        
        return true;
    }

    public async Task<bool> Delete(Guid id)
    {
        var foundedMember = await _dbContext.MemberOfEventEntities
            .FirstOrDefaultAsync(e => e.Id == id);

        if (foundedMember == null)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> DeleteByEventIdAndUserId(Guid eventId, Guid userId)
    {
        var foundedMember = await _dbContext.MemberOfEventEntities
            .FirstOrDefaultAsync(m => m.EventId == eventId && m.UserId == userId);
        if (foundedMember == null)
        {
            return false;
        }

        _dbContext.MemberOfEventEntities.Remove(foundedMember);
        return true;
    }
}