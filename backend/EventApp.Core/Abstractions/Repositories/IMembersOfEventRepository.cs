using EventApp.Core.Models;

namespace EventApp.Core.Abstractions.Repositories;

public interface IMembersOfEventRepository
{
    Task<List<MemberOfEvent>> Get();
    Task<MemberOfEvent> GetById(Guid id);
    Task<List<MemberOfEvent>> GetByEventId(Guid eventId);
    Task<List<MemberOfEvent>> GetByUserId(Guid userId);
    Task<Guid> Create(MemberOfEvent memberOfEvent);
    
    Task<bool> Update(MemberOfEvent memberOfEvent);

    Task<bool> Delete(Guid id);
    Task<bool> DeleteByEventIdAndUserId(Guid eventId, Guid userId);
}