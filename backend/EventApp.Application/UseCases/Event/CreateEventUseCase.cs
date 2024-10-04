using AutoMapper;
using EventApp.Application.DTOs.Event;
using EventApp.DataAccess.Abstractions;
using EventApp.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace EventApp.Application.UseCases.Event
{
    public class CreateEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public CreateEventUseCase(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<Guid> Execute(EventsRequestDto receivedEvent, IFormFile imageFile)
        {
            var newEvent = _mapper.Map<Core.Models.Event>(receivedEvent);

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

                newEvent.ImageUrl = await _imageService.SaveImageToFileSystem(imageFile);
            }

            var id = await _unitOfWork.Events.AddAsync(newEvent);
            await _unitOfWork.Complete();
            return id;
        }
    }
}