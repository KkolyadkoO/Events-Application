using AutoMapper;
using EventApp.Application.DTOs.Event;
using EventApp.Core.Abstractions.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EventApp.Application.UseCases.Event
{
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

            if (imageFile != null)
            {
                try
                {
                    newEvent.ImageUrl = await SaveImageToFileSystem(imageFile);
                }
                catch (InvalidOperationException e)
                {
                    throw new ApplicationException("Image file could not be saved.", e);
                }
                
            }

            var id = await _unitOfWork.Events.Create(newEvent);
            await _unitOfWork.Complete();
            return id;
        }

        private async Task<string> SaveImageToFileSystem(IFormFile imageFile)
        {
            string imagePath = "";
        
            if (imageFile != null && imageFile.Length > 0){

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };

                var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                var mimeType = imageFile.ContentType;

                if (!allowedExtensions.Contains(fileExtension) || !allowedMimeTypes.Contains(mimeType))
                {
                    throw new InvalidOperationException("The uploaded file is not a valid image.");
                }

                var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            
                if (!Directory.Exists(imageFolder))
                {
                    Directory.CreateDirectory(imageFolder);
                }
            
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            
                var filePath = Path.Combine(imageFolder, uniqueFileName);
            
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
            
                return $"/images/{uniqueFileName}";
            }

            return "";
        }
    }
}