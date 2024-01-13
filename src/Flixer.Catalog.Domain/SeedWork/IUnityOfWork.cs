namespace Flixer.Catalog.Domain.SeedWork;

public interface IUnityOfWork
{
    Task Commit(CancellationToken cancellationToken);
    Task RollBack(CancellationToken cancellationToken);
}
