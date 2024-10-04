using Microsoft.AspNetCore.Http;

namespace EventApp.Infrastructure;

public class ImageService : IImageService
{
    public async Task<string> SaveImageToFileSystem(IFormFile imageFile)
    {
        string imagePath = "";

        if (imageFile != null && imageFile.Length > 0)
        {
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

    public async Task<string> UpdateImageToFileSystem(IFormFile imageFile, string oldImageUrl)
    {
        if (imageFile != null && imageFile.Length > 0)
        {
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
                if (File.Exists(oldImagePath))
                {
                    File.Delete(oldImagePath);
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