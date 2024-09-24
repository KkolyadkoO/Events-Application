using EventApp.Core.Abstractions;
using EventApp.Core.Models;

namespace EventApp.Application;

public class LocationOfEventsService : ILocationOfEventsService
{
    private readonly IUnitOfWork _unitOfWork;

    public LocationOfEventsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<LocationOfEvent>> GetAllLocationOfEvents()
    {
        return await _unitOfWork.Locations.Get();
    }

    public async Task<LocationOfEvent> GetLocationOfEventById(Guid id)
    {
        return await _unitOfWork.Locations.GetById(id);
    }

    public async Task<Guid> AddLocationOfEvent(LocationOfEvent locationOfEvent)
    {
        try
        {
            var id = await _unitOfWork.Locations.Add(locationOfEvent.Id, locationOfEvent.Title);
            _unitOfWork.Complete();
            return id;
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException("Failed to add the location: " + ex.Message);
        }
    }

    public async Task<Guid> UpdateLocationOfEvent(Guid id, string title)
    {
        return await _unitOfWork.Locations.Update(id, title);
    }

    public async Task<Guid> DeleteLocationOfEvent(Guid id)
    {
        return await _unitOfWork.Locations.Delete(id);
    }
}