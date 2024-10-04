using EventApp.Core.Models;

namespace EventApp.Core.Abstractions.Repositories;

public interface IMembersOfEventRepository : IRepository<MemberOfEvent>
{
    Task<List<MemberOfEvent>> GetByEventIdAsync(Guid eventId);
    Task<List<MemberOfEvent>> GetByUserIdAsync(Guid userId);
    Task DeleteByEventIdAndUserIdAsync(Guid eventId, Guid userId);
    Task<MemberOfEvent> GetByEventIdAndUserIdAsync(Guid eventId, Guid userId);
}