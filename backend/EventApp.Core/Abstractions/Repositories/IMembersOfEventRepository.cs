using EventApp.Core.Models;

namespace EventApp.DataAccess.Repositories;

public interface IMembersOfEventRepository
{
    Task<List<MemberOfEvent>> Get();
    Task<MemberOfEvent> GetById(Guid id);
    Task<List<MemberOfEvent>> GetByEventId(Guid eventId);
    Task<List<MemberOfEvent>> GetByUserId(Guid userId);
    Task<Guid> Create(MemberOfEvent memberOfEvent);

    Task<Guid> Create(Guid id, string name, DateTime birthday,
        DateTime dateOfRegistration, string email, string lastName, Guid eventId, Guid userId);

    Task<Guid> Update(Guid id, string name, DateTime birthday,
        string email, string lastName, Guid eventId, Guid userId);

    Task<Guid> Delete(Guid id);
    Task DeleteByEventIdAndUserId(Guid eventId, Guid userId);
}