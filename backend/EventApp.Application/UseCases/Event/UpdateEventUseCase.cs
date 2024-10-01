using AutoMapper;
using EventApp.Application.DTOs.Event;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;
using Microsoft.AspNetCore.Http;

namespace EventApp.Application.UseCases.Event;

public class UpdateEventUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEventUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id,EventsRequestDto request, IFormFile? imageFile)
    {
        var existingEvent = await _unitOfWork.Events.GetById(id);
        if (existingEvent == null)
        {
            throw new NotFoundException("Event not found");
        }

        existingEvent.Title = request.Title;
        existingEvent.LocationId = request.LocationId;
        existingEvent.Date = request.Date;
        existingEvent.CategoryId = request.CategoryId;
        existingEvent.Description = request.Description;
        existingEvent.MaxNumberOfMembers = request.MaxNumberOfMembers;

        if (imageFile != null)
        {
            existingEvent.Image = await ConvertImageToBytes(imageFile);
        }

        await _unitOfWork.Events.Update(existingEvent);
        await _unitOfWork.Complete();
    }

    private async Task<byte[]> ConvertImageToBytes(IFormFile imageFile)
    {
        using var memoryStream = new MemoryStream();
        await imageFile.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}
