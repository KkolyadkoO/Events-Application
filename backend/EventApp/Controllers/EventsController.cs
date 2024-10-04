using EventApp.Application.DTOs.Event;
using EventApp.Application.Exceptions;
using EventApp.Application.UseCases.Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly CreateEventUseCase _createEventUseCase;
    private readonly UpdateEventUseCase _updateEventUseCase;
    private readonly DeleteEventUseCase _deleteEventUseCase;
    private readonly GetEventByIdUseCase _getEventByIdUseCase;
    private readonly GetEventsByFiltersUseCase _getEventsByFiltersUseCase;

    public EventsController(CreateEventUseCase createEventUseCase,
        UpdateEventUseCase updateEventUseCase,
        DeleteEventUseCase deleteEventUseCase,
        GetEventByIdUseCase getEventByIdUseCase,
        GetEventsByFiltersUseCase getEventsByFiltersUseCase
    )
    {
        _createEventUseCase = createEventUseCase;
        _updateEventUseCase = updateEventUseCase;
        _deleteEventUseCase = deleteEventUseCase;
        _getEventByIdUseCase = getEventByIdUseCase;
        _getEventsByFiltersUseCase = getEventsByFiltersUseCase;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EventsResponseDto>> GetEventById(Guid id)
    {
        var foundEvent = await _getEventByIdUseCase.Execute(id);

        return Ok(foundEvent);
    }

    [HttpGet("filter/")]
    public async Task<ActionResult> GetFilterEvents([FromQuery] EventFilterRequestDto filterRequest)
    {
        var (events, countOfEvents) = await _getEventsByFiltersUseCase.Execute(filterRequest);

        return Ok(new
        {
            Events = events,
            TotalEventCount = countOfEvents
        });
    }

    [HttpPost]
    // [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<Guid>> CreateEvent([FromForm] EventsRequestDto request, IFormFile imageFile)
    {
        try
        {
            var id = await _createEventUseCase.Execute(request, imageFile);
            return Ok(id);
        }
        catch (ApplicationException e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPut("{id:guid}")]
    // [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<Guid>> UpdateEvent(Guid id, [FromForm] EventsRequestDto request,
        IFormFile? imageFile)
    {
        try
        {
            await _updateEventUseCase.Execute(id, request, imageFile);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(new { message = e.Message });
        }
        catch (ApplicationException e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    // [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<Guid>> DeleteEvent(Guid id)
    {
        try
        {
            await _deleteEventUseCase.Execute(id);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(new { message = e.Message });
        }
    }
}