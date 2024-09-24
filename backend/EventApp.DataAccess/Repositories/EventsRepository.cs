using AutoMapper;
using EventApp.Core.Exceptions;
using EventApp.Core.Models;
using EventApp.DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EventApp.DataAccess.Repositories;

public class EventsRepository : IEventsRepository
{
    private readonly EventAppDBContext _dbContex;
    private readonly IMapper _mapper;

    public EventsRepository(EventAppDBContext dbContext, IMapper mapper)
    {
        _dbContex = dbContext;
        _mapper = mapper;
    }

    public async Task<List<Event>> Get()
    {
        var eventEntities = await _dbContex.EventEntities
            .AsNoTracking()
            .Include(e => e.Members)
            .OrderBy(e => e.Date)
            .ToListAsync();
        return _mapper.Map<List<Event>>(eventEntities);
    }

    public async Task<Event?> GetById(Guid id)
    {
        var findedEvent = await _dbContex.EventEntities
            .AsNoTracking()
            .Include(e => e.Members)
            .FirstOrDefaultAsync(e => e.Id == id);
        return _mapper.Map<Event>(findedEvent);
    }

    public async Task<Event?> GetByTitle(string title)
    {
        var findedEvent = await _dbContex.EventEntities
            .AsNoTracking()
            .Include(e => e.Members)
            .FirstOrDefaultAsync(e => e.Title == title);
        return _mapper.Map<Event?>(findedEvent);
    }

    public async Task<(List<Event?>, int)> GetByFilters(string? title, Guid? locationId, DateTime? startDate,
        DateTime? endDate, Guid? categoryId, Guid? userId, int? page, int? size)
    {
        var query = _dbContex.EventEntities.AsNoTracking();

        if (userId.HasValue && userId != Guid.Empty)
        {
            query = query.Where(e => e.Members.Any(m => m.UserId == userId));
        }

        if (!string.IsNullOrWhiteSpace(title))
        {
            query = query.Where(e => e.Title.ToLower().Contains(title.ToLower()));
        }

        if (locationId.HasValue && locationId != Guid.Empty)
        {
            query = query.Where(e => e.LocationId == locationId.Value);
        }

        if (categoryId.HasValue && categoryId != Guid.Empty)
        {
            query = query.Where(e => e.CategoryId == categoryId.Value);
        }

        if (startDate.HasValue)
        {
            query = query.Where(e => e.Date >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(e => e.Date <= endDate.Value);
        }

        query = query.OrderBy(e => e.Date);
        var countOfEvents = query.Count();

        if (page.HasValue && size.HasValue)
        {
            query = query.Skip((int)((page - 1) * size)).Take((int)size);
        }

        var events = await query
            .AsNoTracking()
            .Include(e => e.Members)
            .ToListAsync();
        return (_mapper.Map<List<Event>>(events), countOfEvents);
    }

    public async Task<List<Event>> GetByPage(int page, int size)
    {
        var events = await _dbContex.EventEntities
            .AsNoTracking()
            .Include(e => e.Members)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
        return _mapper.Map<List<Event>>(events);
    }

    public async Task<Guid> Create(Event receivedEvent, IFormFile imageFile)
    {
        var foundedCategory = _dbContex.CategoryOfEventEntities
                                  .AsNoTracking()
                                  .FirstOrDefault(e => e.Id == receivedEvent.CategoryId)
                              ?? throw new InvalidOperationException("Category with the same Id does not exists.");
        var foundedLocation = _dbContex.LocationsOfEventEntities
                                  .AsNoTracking()
                                  .FirstOrDefault(e => e.Id == receivedEvent.LocationId)
                              ?? throw new InvalidOperationException("Location with the same Id does not exists.");
        
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
            
            imagePath = $"/images/{uniqueFileName}";
        }
        
        var newEvent = new EventEntity
        {
            Id = receivedEvent.Id,
            Title = receivedEvent.Title,
            LocationId = foundedLocation.Id,
            Date = receivedEvent.Date,
            CategoryId = foundedCategory.Id,
            Description = receivedEvent.Description,
            MaxNumberOfMembers = receivedEvent.MaxNumberOfMembers,
            ImageUrl = imagePath,
        };
        await _dbContex.EventEntities.AddAsync(newEvent);

        return newEvent.Id;
    }

    public async Task<Guid> Update(Guid id, string title, Guid location, DateTime date, Guid category,
        string description, int maxNumberOfMembers, IFormFile? imageFile)
    {
        var foundedCategory = _dbContex.CategoryOfEventEntities
                                  .AsNoTracking()
                                  .FirstOrDefault(e => e.Id == category)
                              ?? throw new InvalidOperationException("Category with the same Id does not exists.");
        var foundedLocation = _dbContex.LocationsOfEventEntities
                                  .AsNoTracking()
                                  .FirstOrDefault(e => e.Id == location)
                              ?? throw new InvalidOperationException("Location with the same Id does not exists.");
        var existingEvent = await _dbContex.EventEntities
        .AsNoTracking()
        .FirstOrDefaultAsync(e => e.Id == id);

    if (existingEvent == null)
    {
        throw new InvalidOperationException("Event with the specified Id does not exist.");
    }

    string imageUrl = existingEvent.ImageUrl;

    if (imageFile != null)
    {
        var uploadPath = Path.Combine("wwwroot", "images");

        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);

        var filePath = Path.Combine(uploadPath, fileName);

        if (!string.IsNullOrEmpty(existingEvent.ImageUrl))
        {
            var oldImagePath = Path.Combine("wwwroot", existingEvent.ImageUrl.TrimStart('/'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
        }

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        imageUrl = $"/images/{fileName}";
    }

    await _dbContex.EventEntities
        .Where(e => e.Id == id)
        .ExecuteUpdateAsync(s =>
            s.SetProperty(c => c.Title, title)
                .SetProperty(c => c.LocationId, foundedLocation.Id)
                .SetProperty(c => c.Date, date)
                .SetProperty(c => c.CategoryId, foundedCategory.Id)
                .SetProperty(c => c.Description, description)
                .SetProperty(c => c.MaxNumberOfMembers, maxNumberOfMembers)
                .SetProperty(c => c.ImageUrl, imageUrl));

    return id;
}

    public async Task<Guid> Delete(Guid id)
    {
        await _dbContex.EventEntities
            .Where(e => e.Id == id)
            .ExecuteDeleteAsync();
        return id;
    }
}