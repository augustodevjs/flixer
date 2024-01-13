namespace Flixer.Catalog.Application.Contracts.UnityOfWork;

public interface IUnityOfWOrk
{
    public Task Commit(CancellationToken cancellationToken);
    public Task Rollback(CancellationToken cancellationToken);
}
