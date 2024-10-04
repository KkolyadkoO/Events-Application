using EventApp.Application.DTOs.MemberOfEvent;
using EventApp.Application.Exceptions;
using EventApp.Application.UseCases.Member;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.Controllers;
[ApiController]
    [Route("api/[controller]")]
    public class MembersOfEventController : ControllerBase
    {
        private readonly GetMemberOfEventById _getMemberOfEventById;
        private readonly GetAllMembersOfEventByEventId _getAllMembersOfEventByEventId;
        private readonly GetAllMembersOfEventByUserId _getAllMembersOfEventByUserId;
        private readonly AddMemberOfEvent _addMemberOfEvent;
        private readonly UpdateMemberOfEvent _updateMemberOfEvent;
        private readonly DeleteMemberOfEvent _deleteMemberOfEvent;
        private readonly DeleteMemberOfEventByEventIdAndUserId _deleteMemberOfEventByEventIdAndUserId;

        public MembersOfEventController(
            GetMemberOfEventById getMemberOfEventById,
            GetAllMembersOfEventByEventId getAllMembersOfEventByEventId,
            GetAllMembersOfEventByUserId getAllMembersOfEventByUserId,
            AddMemberOfEvent addMemberOfEvent,
            UpdateMemberOfEvent updateMemberOfEvent,
            DeleteMemberOfEvent deleteMemberOfEvent,
            DeleteMemberOfEventByEventIdAndUserId deleteMemberOfEventByEventIdAndUserId)
        {
            _getMemberOfEventById = getMemberOfEventById;
            _getAllMembersOfEventByEventId = getAllMembersOfEventByEventId;
            _getAllMembersOfEventByUserId = getAllMembersOfEventByUserId;
            _addMemberOfEvent = addMemberOfEvent;
            _updateMemberOfEvent = updateMemberOfEvent;
            _deleteMemberOfEvent = deleteMemberOfEvent;
            _deleteMemberOfEventByEventIdAndUserId = deleteMemberOfEventByEventIdAndUserId;
        }

        // GET: api/MembersOfEvent/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var member = await _getMemberOfEventById.Execute(id);
                return Ok(member);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/MembersOfEvent/event/{eventId}
        [HttpGet("event/{eventId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetByEventId(Guid eventId)
        {
            var members = await _getAllMembersOfEventByEventId.Execute(eventId);
            return Ok(members);
        }

        // GET: api/MembersOfEvent/user/{userId}
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var members = await _getAllMembersOfEventByUserId.Execute(userId);
            return Ok(members);
        }

        // POST: api/MembersOfEvent
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] MemberOfEventsRequestDto request)
        {
            var newMemberId = await _addMemberOfEvent.Execute(request);
            return CreatedAtAction(nameof(GetById), new { id = newMemberId }, new { id = newMemberId });
        }

        // PUT: api/MembersOfEvent/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] MemberOfEventsRequestDto request)
        {
            try
            {
                await _updateMemberOfEvent.Execute(id, request);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/MembersOfEvent/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _deleteMemberOfEvent.Execute(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/MembersOfEvent/event/{eventId}/user/{userId}
        [HttpDelete("event/{eventId}/user/{userId}")]
        [Authorize]
        public async Task<IActionResult> DeleteByEventIdAndUserId(Guid eventId, Guid userId)
        {
            try
            {
                await _deleteMemberOfEventByEventIdAndUserId.Execute(eventId, userId);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }