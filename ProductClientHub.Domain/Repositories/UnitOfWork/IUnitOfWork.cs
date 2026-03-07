namespace ProductClientHub.Domain.Repositories.UnitOfWork;

public interface IUnitOfWork
{
    Task Commit();
}
