using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;

namespace EventApp.Application.UseCases.Member;

public class DeleteMemberOfEvent
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMemberOfEvent(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id)
    {
        var memberOfEvent = await _unitOfWork.Members.GetById(id);
        if (memberOfEvent == null)
        {
            throw new NotFoundException($"Member of Event with Id {id} not found");
        }

        await _unitOfWork.Members.Delete(id);
        await _unitOfWork.Complete();
    }
}