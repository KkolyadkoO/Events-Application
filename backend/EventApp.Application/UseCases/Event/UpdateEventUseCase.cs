using AutoMapper;
using EventApp.Application.DTOs.Event;
using EventApp.Application.Exceptions;
using EventApp.DataAccess.Abstractions;
using EventApp.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace EventApp.Application.UseCases.Event;

public class UpdateEventUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;

    public UpdateEventUseCase(IUnitOfWork unitOfWork, IImageService imageService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _mapper = mapper;
    }

    public async Task Execute(Guid id, EventsRequestDto request, IFormFile? imageFile)
    {
        var existingEvent = await _unitOfWork.Events.GetByIdAsync(id);
        if (existingEvent == null)
        {
            throw new NotFoundException("Event not found");
        }

        var updatedEvent = _mapper.Map<Core.Models.Event>(request);

        if (imageFile != null)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };

            var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            var mimeType = imageFile.ContentType;

            if (!allowedExtensions.Contains(fileExtension) || !allowedMimeTypes.Contains(mimeType))
            {
                throw new InvalidOperationException("The uploaded file is not a valid image.");
            }

            updatedEvent.ImageUrl = await _imageService.UpdateImageToFileSystem(imageFile, existingEvent.ImageUrl);
        }
        updatedEvent.Id = id;

        await _unitOfWork.Events.UpdateAsync(updatedEvent);
        await _unitOfWork.Complete();
    }
}