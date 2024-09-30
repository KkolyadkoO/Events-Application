using AutoMapper;
using EventApp.Core.Abstractions.Repositories;
using EventApp.DataAccess.Repositories;

namespace EventApp.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly EventAppDBContext _context;
    private readonly IMapper _mapper;

    public ICategoryOfEventsRepository Categories { get; private set; }
    public ILocationOfEventsRepository Locations { get; private set; }
    public IEventsRepository Events { get; private set; }
    public IMembersOfEventRepository Members { get; private set; }
    public IUserRepository Users { get; private set; }
    
    public IRefreshTokenRepository RefreshTokens { get; private set; }
    

    public UnitOfWork(EventAppDBContext context)
    {
        _context = context;
        Categories = new CategoryOfEventsRepository(_context);
        Events = new EventsRepository(_context);
        Members = new MembersOfEventRepository(_context);
        Users = new UserRepository(_context);
        RefreshTokens = new RefreshTokenRepository(_context);
        Locations = new LocationOfEventsRepository(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }
}