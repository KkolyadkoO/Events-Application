using System.Linq.Expressions;

namespace EventApp.Core.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; }
    int? Skip { get; }
    int? Take { get; }
}