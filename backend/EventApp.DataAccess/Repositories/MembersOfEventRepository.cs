using AutoMapper;
using EventApp.Core.Models;
using EventApp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventApp.DataAccess.Repositories;

public class MembersOfEventRepository : IMembersOfEventRepository
{
    private readonly EventAppDBContext _dbContex;
    private readonly IMapper _mapper;
    
    public MembersOfEventRepository(EventAppDBContext dbContext, IMapper mapper)
    {
        _dbContex = dbContext;
        _mapper = mapper;
    }

    public async Task<List<MemberOfEvent>> Get()
    {
        var memberOfEvents = await _dbContex.MemberOfEventEntities
            .AsNoTracking()
            .ToListAsync();
        return _mapper.Map<List<MemberOfEvent>>(memberOfEvents);
    }

    public async Task<MemberOfEvent> GetById(Guid id)
    {
        var memberOfEvent = await _dbContex.MemberOfEventEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
        return _mapper.Map<MemberOfEvent>(memberOfEvent);
    }

    public async Task<List<MemberOfEvent>> GetByEventId(Guid eventId)
    {
        var query = _dbContex.MemberOfEventEntities.AsNoTracking();
        if (eventId != Guid.Empty)
        {
            query = query.Where(m => m.EventId == eventId);
        }
        var members = await query.ToListAsync();
        return _mapper.Map<List<MemberOfEvent>>(members);
    }

    public async Task<List<MemberOfEvent>> GetByUserId(Guid userId)
    {
        var query = _dbContex.MemberOfEventEntities.AsNoTracking();
        if (userId != Guid.Empty)
        {
            query.Where(m => m.UserId == userId);
        }
        var members = await query.ToListAsync();
        return _mapper.Map<List<MemberOfEvent>>(members);
    }

    public async Task<Guid> Create(MemberOfEvent memberOfEvent)
    {
        var member = new MemberOfEventEntity
        {
            UserId = memberOfEvent.UserId,
            Birthday = memberOfEvent.Birthday,
            DateOfRegistration = memberOfEvent.DateOfRegistration,
            Email = memberOfEvent.Email,
            Id = memberOfEvent.Id,
            Name = memberOfEvent.Name,
            LastName = memberOfEvent.LastName,
            EventId = memberOfEvent.EventId
        };
        await _dbContex.MemberOfEventEntities.AddAsync(member);
        return member.Id;
    }
    public async Task<Guid> Create(Guid id, string name, DateTime birthday,
        DateTime dateOfRegistration, string email, string lastName, Guid eventId, Guid userId)
    {
        var member = new MemberOfEventEntity
        {
            Id = id,
            Name = name,
            LastName = lastName,
            Birthday = birthday,
            DateOfRegistration = dateOfRegistration,
            Email = email,
            EventId = eventId,
            UserId = userId
        };
        await _dbContex.MemberOfEventEntities.AddAsync(member);
        return member.Id;
    }
    public async Task<Guid> Update(Guid id, string name, DateTime birthday, string email, string lastName, Guid eventId, Guid userId)
    {

        await _dbContex.MemberOfEventEntities
            .Where(m => m.Id == id)
            .ExecuteUpdateAsync(m =>
                m.SetProperty(m => m.Name, name)
                    .SetProperty(m => m.LastName, lastName)
                    .SetProperty(m => m.Birthday, birthday)
                    .SetProperty(m => m.Email, email)
                    .SetProperty(m => m.Id, eventId)
                    .SetProperty(m => m.UserId, userId));
        return id;
    }

    public async Task<Guid> Delete(Guid id)
    {
        await _dbContex.MemberOfEventEntities
            .Where(m => m.Id == id)
            .ExecuteDeleteAsync();
        return id;
    }

    public async Task DeleteByEventIdAndUserId(Guid eventId, Guid userId)
    {
        await _dbContex.MemberOfEventEntities
            .Where(m => m.EventId == eventId && m.UserId == userId)
            .ExecuteDeleteAsync();
    }
}