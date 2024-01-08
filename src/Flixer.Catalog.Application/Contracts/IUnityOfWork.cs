namespace Flixer.Catalog.Application.Contracts;

public interface IUnityOfWork
{
    Task Commit(CancellationToken cancellationToken);
    Task RollBack(CancellationToken cancellationToken);
}
