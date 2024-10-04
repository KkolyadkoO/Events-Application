using EventApp.Application.Exceptions;
using EventApp.DataAccess.Abstractions;

namespace EventApp.Application.UseCases.Member;

public class DeleteMemberOfEventByEventIdAndUserId
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMemberOfEventByEventIdAndUserId(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid eventId, Guid userId)
    {
        var memberOfEvent = await _unitOfWork.Members.GetByEventIdAndUserIdAsync(eventId, userId);
        if (memberOfEvent == null)
        {
            throw new NotFoundException($"Member of Event with EventId {eventId} and UserId {userId} not found");
        }

        await _unitOfWork.Members.DeleteByEventIdAndUserIdAsync(eventId, userId);
        await _unitOfWork.Complete();
    }
}