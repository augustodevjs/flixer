namespace Flixer.Catalog.Application.Contracts;
public interface IUnityOfWork
{
    Task Commit(CancellationToken cancellationToken);
}
