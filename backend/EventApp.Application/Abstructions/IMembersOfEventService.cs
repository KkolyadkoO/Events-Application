using EventApp.Core.Models;

namespace EventApp.Application;

public interface IMembersOfEventService
{
    Task<List<MemberOfEvent>> GetAllMemberOfEvents();
    Task<MemberOfEvent?> GetMemberOfEventById(Guid id);
    Task<List<MemberOfEvent>> GetAllMembersOfEventByEventId(Guid eventId);
    Task<List<MemberOfEvent>> GetAllMembersOfUserById(Guid userId);

    Task<Guid> AddMemberOfEvent(Guid id, string name, DateTime birthday,
        DateTime dateOfRegistration, string email, string lastName, Guid eventId, Guid userId);

    Task<Guid> AddMemberOfEvent(MemberOfEvent memberOfEvent);

    Task<Guid> UpdateMemberOfEvent(Guid id, string name, DateTime birthday,
        string email, string lastName, Guid eventId, Guid userId);

    Task<Guid> DeleteMemberOfEvent(Guid id);
    Task DeleteMemberOfEventByEventIdAndUserId(Guid eventId, Guid userId);
}