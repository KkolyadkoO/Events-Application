namespace EventApp.Application;

public interface IUpdateCategoryUseCase
{
    Task<Guid> Execute(Guid id, string title);
}