using EventApp.Application;
using EventApp.Core.Abstractions;
using EventApp.Core.Models;
using EventApp.DataAccess.Repositories;
using Moq;
using Xunit;

namespace EventApp.Tests.Services;

public class MembersOfEventServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMembersOfEventRepository> _membersRepositoryMock;
    private readonly MembersOfEventService _membersOfEventService;

    public MembersOfEventServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _membersRepositoryMock = new Mock<IMembersOfEventRepository>();
        
        _unitOfWorkMock.Setup(u => u.Members).Returns(_membersRepositoryMock.Object);
        _membersOfEventService = new MembersOfEventService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetAllMemberOfEvents_ReturnsListOfMembers()
    {
        var members = new List<MemberOfEvent>
        {
            new MemberOfEvent(Guid.NewGuid(), "John", "Doe", DateTime.Now, DateTime.Now, "john@example.com", Guid.NewGuid(), Guid.NewGuid()),
            new MemberOfEvent(Guid.NewGuid(), "Jane", "Doe", DateTime.Now, DateTime.Now, "jane@example.com", Guid.NewGuid(), Guid.NewGuid())
        };

        _membersRepositoryMock.Setup(m => m.Get()).ReturnsAsync(members);

        var result = await _membersOfEventService.GetAllMemberOfEvents();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetMemberOfEventById_ReturnsMember()
    {
        var memberId = Guid.NewGuid();
        var member = new MemberOfEvent(memberId, "John", "Doe", DateTime.Now, DateTime.Now, "john@example.com", Guid.NewGuid(), Guid.NewGuid());

        _membersRepositoryMock.Setup(m => m.GetById(memberId)).ReturnsAsync(member);

        var result = await _membersOfEventService.GetMemberOfEventById(memberId);

        Assert.NotNull(result);
        Assert.Equal(memberId, result.Id);
    }

    [Fact]
    public async Task GetAllMembersOfEventByEventId_ReturnsListOfMembers()
    {
        var eventId = Guid.NewGuid();
        var members = new List<MemberOfEvent>
        {
            new MemberOfEvent(Guid.NewGuid(), "John", "Doe", DateTime.Now, DateTime.Now, "john@example.com", Guid.NewGuid(), eventId),
            new MemberOfEvent(Guid.NewGuid(), "Jane", "Doe", DateTime.Now, DateTime.Now, "jane@example.com", Guid.NewGuid(), eventId)
        };

        _membersRepositoryMock.Setup(m => m.GetByEventId(eventId)).ReturnsAsync(members);

        var result = await _membersOfEventService.GetAllMembersOfEventByEventId(eventId);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllMembersOfUserById_ReturnsListOfMembers()
    {
        var userId = Guid.NewGuid();
        var members = new List<MemberOfEvent>
        {
            new MemberOfEvent(Guid.NewGuid(), "John", "Doe", DateTime.Now, DateTime.Now, "john@example.com", userId, Guid.NewGuid()),
            new MemberOfEvent(Guid.NewGuid(), "Jane", "Doe", DateTime.Now, DateTime.Now, "jane@example.com", userId, Guid.NewGuid())
        };

        _membersRepositoryMock.Setup(m => m.GetByUserId(userId)).ReturnsAsync(members);

        var result = await _membersOfEventService.GetAllMembersOfUserById(userId);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task AddMemberOfEvent_CreatesMemberAndReturnsId()
    {
        var memberId = Guid.NewGuid();
        var member = new MemberOfEvent(memberId, "John", "Doe", DateTime.Now, DateTime.Now, "john@example.com", Guid.NewGuid(), Guid.NewGuid());

        _membersRepositoryMock.Setup(m => m.Create(member)).ReturnsAsync(memberId);

        var result = await _membersOfEventService.AddMemberOfEvent(member);

        Assert.Equal(memberId, result);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }

    [Fact]
    public async Task AddMemberOfEvent_WithParams_CreatesMemberAndReturnsId()
    {
        var memberId = Guid.NewGuid();
        var name = "John";
        var lastName = "Doe";
        var birthday = DateTime.Now;
        var dateOfRegistration = DateTime.Now;
        var email = "john@example.com";
        var eventId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _membersRepositoryMock.Setup(m => m.Create(memberId, name, birthday, dateOfRegistration, email, lastName, eventId, userId))
            .ReturnsAsync(memberId);

        var result = await _membersOfEventService.AddMemberOfEvent(memberId, name, birthday, dateOfRegistration, email, lastName, eventId, userId);

        Assert.Equal(memberId, result);
    }

    [Fact]
    public async Task UpdateMemberOfEvent_UpdatesMemberAndReturnsId()
    {
        var memberId = Guid.NewGuid();
        var name = "John";
        var lastName = "Doe";
        var birthday = DateTime.Now;
        var email = "john@example.com";
        var eventId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _membersRepositoryMock.Setup(m => m.Update(memberId, name, birthday, email, lastName, eventId, userId))
            .ReturnsAsync(memberId);

        var result = await _membersOfEventService.UpdateMemberOfEvent(memberId, name, birthday, email, lastName, eventId, userId);

        Assert.Equal(memberId, result);
    }

    [Fact]
    public async Task DeleteMemberOfEvent_DeletesMemberAndReturnsId()
    {
        var memberId = Guid.NewGuid();

        _membersRepositoryMock.Setup(m => m.Delete(memberId)).ReturnsAsync(memberId);

        var result = await _membersOfEventService.DeleteMemberOfEvent(memberId);

        Assert.Equal(memberId, result);
    }

    [Fact]
    public async Task AddMemberOfEvent_ThrowsInvalidOperationException_OnFailure()
    {
        var member = new MemberOfEvent(Guid.NewGuid(), "John", "Doe", DateTime.Now, DateTime.Now, "john@example.com", Guid.NewGuid(), Guid.NewGuid());

        _membersRepositoryMock.Setup(m => m.Create(member)).ThrowsAsync(new InvalidOperationException("Test exception"));

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _membersOfEventService.AddMemberOfEvent(member));

        Assert.Equal("Test exception", exception.Message);
    }

    [Fact]
    public async Task UpdateMemberOfEvent_ThrowsInvalidOperationException_OnFailure()
    {
        var memberId = Guid.NewGuid();
        var name = "John";
        var lastName = "Doe";
        var birthday = DateTime.Now;
        var email = "john@example.com";
        var eventId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _membersRepositoryMock.Setup(m => m.Update(memberId, name, birthday, email, lastName, eventId, userId))
            .ThrowsAsync(new InvalidOperationException("Test exception"));


        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _membersOfEventService.UpdateMemberOfEvent(memberId, name, birthday, email, lastName, eventId, userId));

        Assert.Equal("Test exception", exception.Message);
    }
}