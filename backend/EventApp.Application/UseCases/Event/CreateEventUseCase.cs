using AutoMapper;
using EventApp.Application.DTOs.Event;
using EventApp.Core.Abstractions.Repositories;
using Microsoft.AspNetCore.Http;

namespace EventApp.Application.UseCases.Event;

public class CreateEventUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateEventUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Execute(EventsRequestDto receivedEvent, IFormFile imageFile)
    {
        var newEvent = _mapper.Map<Core.Models.Event>(receivedEvent);
        newEvent.Image = await ConvertImageToBytes(imageFile);
            
        var id = await _unitOfWork.Events.Create(newEvent);
        await _unitOfWork.Complete();
        return id;
    }

    private async Task<byte[]> ConvertImageToBytes(IFormFile imageFile)
    {
        if (imageFile == null) return null;

        using var memoryStream = new MemoryStream();
        await imageFile.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}