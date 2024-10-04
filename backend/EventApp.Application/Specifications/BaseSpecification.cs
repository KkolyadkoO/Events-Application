using System.Linq.Expressions;
using EventApp.Core.Abstractions;

namespace EventApp.Application.Specifications;

public class BaseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>> Criteria { get; private set; }
    public Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; private set; }
    public int? Skip { get; private set; }
    public int? Take { get; private set; }

    public BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    public void AddOrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    public void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
    }
}