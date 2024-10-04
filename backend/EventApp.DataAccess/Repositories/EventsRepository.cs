using EventApp.Core.Abstractions.Repositories;
using EventApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using EventApp.Core.Abstractions;

namespace EventApp.DataAccess.Repositories
{
    public class EventsRepository : Repository<Event>, IEventsRepository
    {
        private readonly EventAppDBContext _dbContext;

        public EventsRepository(EventAppDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<List<Event>> GetAllAsync()
        {
            return await _dbContext.EventEntities
                .AsNoTracking()
                .Include(e => e.Members)
                .OrderBy(e => e.Date)
                .ToListAsync();
        }

        public override async Task<Event> GetByIdAsync(Guid id)
        {
            return await _dbContext.EventEntities
                .AsNoTracking()
                .Include(e => e.Members)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<(List<Event>, int)> GetBySpecificationAsync(ISpecification<Event> spec, int? page, int? size)
        {
            var query = ApplySpecification(spec, _dbContext.EventEntities);

            var countOfEvents = await query.CountAsync();

            if (page.HasValue && size.HasValue)
            {
                query = query.Skip((int)((page - 1) * size)).Take((int)size);
            }

            var events = await query
                .Include(e => e.Members)
                .AsNoTracking()
                .ToListAsync();

            return (events, countOfEvents);
        }
        

        private IQueryable<Event> ApplySpecification(ISpecification<Event> spec, IQueryable<Event> query)
        {
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.OrderBy != null)
            {
                query = spec.OrderBy(query);
            }

            return query;
        }
    }
}
