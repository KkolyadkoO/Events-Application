using EventApp.DataAccess.Repositories;

namespace EventApp.Core.Abstractions;

public interface IUnitOfWork : IDisposable
{
    ICategoryOfEventsRepository Categories { get; }
    IEventsRepository Events { get; }
    IMembersOfEventRepository Members { get; }
    IUserRepository Users { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    ILocationOfEventsRepository Locations { get; }
    Task<int> Complete();
}