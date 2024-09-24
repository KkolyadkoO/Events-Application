using EventApp.Core.Abstractions;
using EventApp.Core.Models;

namespace EventApp.Application;

public class MembersOfEventService : IMembersOfEventService
{
    private readonly IUnitOfWork _unitOfWork;

    public MembersOfEventService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<MemberOfEvent>> GetAllMemberOfEvents()
    {
        return await _unitOfWork.Members.Get();
    }

    public async Task<MemberOfEvent?> GetMemberOfEventById(Guid id)
    {
        return await _unitOfWork.Members.GetById(id);
    }

    public async Task<List<MemberOfEvent>> GetAllMembersOfEventByEventId(Guid eventId)
    {
        return await _unitOfWork.Members.GetByEventId(eventId);
    }
    public async Task<List<MemberOfEvent>> GetAllMembersOfUserById(Guid userId)
    {
        return await _unitOfWork.Members.GetByUserId(userId);
    }

    public async Task<Guid> AddMemberOfEvent(Guid id, string name, DateTime birthday,
        DateTime dateOfRegistration, string email, string lastName, Guid eventId, Guid userId)
    {
        return await _unitOfWork.Members.Create(id, name, birthday, dateOfRegistration, email, lastName, eventId, userId);
    }
    public async Task<Guid> AddMemberOfEvent(MemberOfEvent memberOfEvent)
    {
        var id = await _unitOfWork.Members.Create(memberOfEvent);
        _unitOfWork.Complete();
        return id;
    }

    public async Task<Guid> UpdateMemberOfEvent(Guid id, string name, DateTime birthday, string email, string lastName, Guid eventId, Guid userId)
    {
        return await _unitOfWork.Members.Update(id, name, birthday, email, lastName, eventId, userId);
    }

    public async Task<Guid> DeleteMemberOfEvent(Guid id)
    {
        return await _unitOfWork.Members.Delete(id);
    }

    public async Task DeleteMemberOfEventByEventIdAndUserId(Guid eventId, Guid userId)
    {
        try
        {
            await _unitOfWork.Members.DeleteByEventIdAndUserId(eventId, userId);
        }
        catch (Exception e)
        {
            throw new Exception("UserId or EventId invalid");
        }
    }
}