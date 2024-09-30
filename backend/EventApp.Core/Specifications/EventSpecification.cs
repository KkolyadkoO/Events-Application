using EventApp.Core.Models;

namespace EventApp.Core.Specifications;

public class EventSpecification : BaseSpecification<Event>
{
    public EventSpecification(string? title, Guid? locationId, DateTime? startDate, DateTime? endDate,
        Guid? categoryId, Guid? userId)
        : base(e => 
            (string.IsNullOrEmpty(title) || e.Title.ToLower().Contains(title.ToLower())) &&
            (!locationId.HasValue || e.LocationId == locationId.Value) &&
            (!categoryId.HasValue || e.CategoryId == categoryId.Value) &&
            (!startDate.HasValue || e.Date >= startDate.Value) &&
            (!endDate.HasValue || e.Date <= endDate.Value) &&
            (!userId.HasValue || e.Members.Any(m => m.UserId == userId.Value))
        )
    {
        AddOrderBy(q => q.OrderBy(e => e.Date));
    }
}