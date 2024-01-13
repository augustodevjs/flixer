namespace Flixer.Catalog.Application.Contracts.UnityOfWork;

public interface IUnityOfWork
{
    public Task Commit(CancellationToken cancellationToken);
    public Task Rollback(CancellationToken cancellationToken);
}
