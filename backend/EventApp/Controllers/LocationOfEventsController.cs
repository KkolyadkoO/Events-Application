using EventApp.Application;
using EventApp.Application.DTOs.CategoryOfEvent;
using EventApp.Application.DTOs.LocationOfEvent;
using EventApp.Application.UseCases.Category;
using EventApp.Application.UseCases.Location;
using EventApp.Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly AddLocationUseCase _addLocationUseCase;
    private readonly DeleteLocationUseCase _deleteLocationUseCase;
    private readonly GetAllLocationsUseCase _getAllLocationsUseCase;
    private readonly GetLocationByIdUseCase _getLocationByIdUseCase;
    private readonly UpdateLocationUseCase _updateLocationUseCase;

    public LocationsController(
        AddLocationUseCase addLocationUseCase,
        DeleteLocationUseCase deleteLocationUseCase,
        GetAllLocationsUseCase getAllLocationsUseCase,
        GetLocationByIdUseCase getLocationByIdUseCase,
        UpdateLocationUseCase updateLocationUseCase)
    {
        _addLocationUseCase = addLocationUseCase;
        _deleteLocationUseCase = deleteLocationUseCase;
        _getAllLocationsUseCase = getAllLocationsUseCase;
        _getLocationByIdUseCase = getLocationByIdUseCase;
        _updateLocationUseCase = updateLocationUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLocations()
    {
        var locations = await _getAllLocationsUseCase.Execute();
        return Ok(locations);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetLocationById(Guid id)
    {
        try
        {
            var location = await _getLocationByIdUseCase.Execute(id);
            return Ok(location);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> AddLocation([FromBody] LocationOfEventsRequestDto requestDto)
    {
        try
        {
            var id = await _addLocationUseCase.Execute(requestDto);
            return CreatedAtAction(nameof(GetLocationById), new { id }, id);
        }
        catch (DuplicateCategory ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateLocation(Guid id, [FromBody] LocationOfEventsRequestDto requestDto)
    {
        try
        {
            await _updateLocationUseCase.Execute(id, requestDto);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteLocation(Guid id)
    {
        try
        {
            await _deleteLocationUseCase.Execute(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}