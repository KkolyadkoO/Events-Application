using AutoMapper;
using EventApp.Application.DTOs.Event;
using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Exceptions;
using Microsoft.AspNetCore.Http;

namespace EventApp.Application.UseCases.Event;

public class UpdateEventUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEventUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id, EventsRequestDto request, IFormFile? imageFile)
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
            try
            {
                existingEvent.ImageUrl = await UpdateImageToFileSystem(imageFile, existingEvent.ImageUrl);
            }
            catch (InvalidOperationException e)
            {
                throw new ApplicationException("Image file could not be updated ", e);
            }
        }

        await _unitOfWork.Events.Update(existingEvent);
        await _unitOfWork.Complete();
    }

    private async Task<string> UpdateImageToFileSystem(IFormFile imageFile, string oldImageUrl)
    {
        string imageUrl = oldImageUrl;
        if (imageFile != null && imageFile.Length > 0)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };

            var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            var mimeType = imageFile.ContentType;

            if (!allowedExtensions.Contains(fileExtension) || !allowedMimeTypes.Contains(mimeType))
            {
                throw new InvalidOperationException("The uploaded file is not a valid image.");
            }

            var uploadPath = Path.Combine("wwwroot", "images");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);

            var filePath = Path.Combine(uploadPath, fileName);

            if (!string.IsNullOrEmpty(oldImageUrl))
            {
                var oldImagePath = Path.Combine("wwwroot", oldImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/images/{fileName}";
        }

        return "";
    }
}