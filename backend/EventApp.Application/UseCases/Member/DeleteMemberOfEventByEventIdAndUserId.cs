using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;

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
        var memberOfEvent = await _unitOfWork.Members.GetByEventIdAndUserId(eventId, userId);
        if (memberOfEvent == null)
        {
            throw new NotFoundException($"Member of Event with EventId {eventId} and UserId {userId} not found");
        }

        await _unitOfWork.Members.DeleteByEventIdAndUserId(eventId, userId);
        await _unitOfWork.Complete();
    }
}