using EventApp.Application;
using EventApp.Contracts;
using EventApp.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CategoryOfEventsController : ControllerBase
{
    private readonly ICategoryOfEventsService _categoryOfEventsService;

    public CategoryOfEventsController(ICategoryOfEventsService categoryOfEventsService)
    {
        _categoryOfEventsService = categoryOfEventsService;
    }
    [HttpGet]
    public async Task<ActionResult<List<CategoryOfEventsResponse>>> GetCategories()
    {
        var events = await _categoryOfEventsService.GetAllCategoryOfEvents();
        var response = events.Select(e => new CategoryOfEventsResponse(e.Id,e.Title));
        
        return Ok(response);
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryOfEventsResponse>> GetCategoryOfEventsById(Guid id)
    {
        var categoryOfEvent = await _categoryOfEventsService.GetCategoryOfEventById(id);
        var response = new CategoryOfEventsResponse(categoryOfEvent.Id, categoryOfEvent.Title);
        return Ok(response);
    }
    [HttpGet("{title}")]
    public async Task<ActionResult<CategoryOfEventsResponse>> GetCategoryOfEventsByTitle(string title)
    {
        var categoryOfEvent = await _categoryOfEventsService.GetCategoryOfEventByTitle(title);
        var response = new CategoryOfEventsResponse(categoryOfEvent.Id, categoryOfEvent.Title);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<Guid>> CreateCategory([FromBody] CategoryOfEventsRequest request)
    {
        var categoryOfEvent = new CategoryOfEvent(
            Guid.NewGuid(),
            request.Title);

        try
        {
            var result = await _categoryOfEventsService.AddCategoryOfEvent(categoryOfEvent);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<Guid>> UpdateCategory(Guid id, [FromBody] CategoryOfEventsRequest request)
    {
        return await _categoryOfEventsService.UpdateCategoryOfEvent(id, request.Title);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<Guid>> DeleteCategory(Guid id)
    {
        return await _categoryOfEventsService.DeleteCategoryOfEvent(id);
    }
    
}
[ApiController]
[Route("api/[controller]")]
public class LocationOfEventsController : ControllerBase
{
    private readonly ILocationOfEventsService _locationOfEventsService;

    public LocationOfEventsController(ILocationOfEventsService locationOfEventsService)
    {
        _locationOfEventsService = locationOfEventsService;
    }
    [HttpGet]
    public async Task<ActionResult<List<LocationOfEventsResponse>>> GetLocations()
    {
        var events = await _locationOfEventsService.GetAllLocationOfEvents();
        var response = events.Select(e => new CategoryOfEventsResponse(e.Id,e.Title));
        
        return Ok(response);
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<LocationOfEventsResponse>> GetLocationOfEventsById(Guid id)
    {
        var categoryOfEvent = await _locationOfEventsService.GetLocationOfEventById(id);
        var response = new LocationOfEventsResponse(categoryOfEvent.Id, categoryOfEvent.Title);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<Guid>> CreateLocation([FromBody] LocationOfEventsRequest request)
    {
        var locationOfEvent = new LocationOfEvent(
            Guid.NewGuid(),
            request.Title);

        try
        {
            var result = await _locationOfEventsService.AddLocationOfEvent(locationOfEvent);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<Guid>> UpdateLocation(Guid id, [FromBody] LocationOfEventsRequest request)
    {
        return await _locationOfEventsService.UpdateLocationOfEvent(id, request.Title);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<Guid>> DeleteLocation(Guid id)
    {
        return await _locationOfEventsService.DeleteLocationOfEvent(id);
    }
    
}