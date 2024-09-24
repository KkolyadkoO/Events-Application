using EventApp.Application;
using EventApp.Contracts;
using EventApp.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.Controllers;
[ApiController]
[Route("api/[controller]")]
public class MembersOfEventController : ControllerBase
{
    private readonly IMembersOfEventService _membersOfEventService;

    public MembersOfEventController(IMembersOfEventService membersOfEventService)
    {
        _membersOfEventService = membersOfEventService;
    }

    [HttpGet("event/{eventId:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<List<MemberOfEventsResponse>>> GetAllMembersOfEventById(Guid eventId)
    {
        var membersOfEvent = await _membersOfEventService.GetAllMembersOfEventByEventId(eventId);
        var response = membersOfEvent.Select(m => new MemberOfEventsResponse(
            m.Id, m.Name, m.LastName, m.Birthday.ToLocalTime(), m.Email, m.UserId, m.EventId));
        if (response == null)
        {
            return BadRequest(new { message = "Event not found." });
        }
        return Ok(response);
    }

    [HttpGet("user/{userId:guid}")]
    [Authorize]
    public async Task<ActionResult<List<MemberOfEventsResponse>>> GetMembersOfEventByUserId(Guid userId)
    {
        var membersOfEvent = await _membersOfEventService.GetAllMembersOfUserById(userId);
        var response = membersOfEvent.Select(m => new MemberOfEventsResponse(
            m.Id, m.Name, m.LastName, m.Birthday.ToLocalTime(), m.Email, m.UserId, m.EventId));
        if (response == null)
        {
            return BadRequest(new { message = "Event not found." });
        }
        return Ok(response);
    }
    [HttpGet("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<MemberOfEventsResponse>> GetMemberOfEventById(Guid id)
    {
        var membersOfEvent = await _membersOfEventService.GetMemberOfEventById(id);
        var response = new MemberOfEventsResponse(
            membersOfEvent.Id, membersOfEvent.Name, membersOfEvent.LastName,
            membersOfEvent.Birthday.ToLocalTime(), membersOfEvent.Email, membersOfEvent.UserId, membersOfEvent.EventId);
        if (response == null)
        {
            return BadRequest(new { message = "Event not found." });
        }
        return Ok(response);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<MemberOfEventsResponse>> AddMember(MemberOfEventsRequest request)
    {
        var memberOfEvent = new MemberOfEvent(
            Guid.NewGuid(), request.Name, request.LastName, request.Birthday.ToUniversalTime(),
            DateTime.Now.ToUniversalTime(), request.Email, request.UserId,request.EventId);
        await _membersOfEventService.AddMemberOfEvent(memberOfEvent);
        return Ok(memberOfEvent.Id);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<Guid>> UpdateMember(Guid id, [FromBody] MemberOfEventsRequest request)
    {
        return await _membersOfEventService.UpdateMemberOfEvent(id, request.Name, request.Birthday.ToUniversalTime()
            , request.Email, request.LastName, request.EventId, request.UserId);

    }
    [HttpDelete]
    [Authorize]
    public async Task<ActionResult> DeleteMemberByEventIdAndUserId([FromQuery]Guid eventId, Guid userId)
    {
        try
        {
            await _membersOfEventService.DeleteMemberOfEventByEventIdAndUserId(eventId, userId);
            return Ok();
        }
        catch (Exception e)
        {
           return BadRequest(e.Message);
        }
        
    }


    [HttpDelete("{Id:guid}")]
    [Authorize]
    public async Task<ActionResult<Guid>> DeleteMember(Guid Id)
    {
        return await _membersOfEventService.DeleteMemberOfEvent(Id);
    }
    
}