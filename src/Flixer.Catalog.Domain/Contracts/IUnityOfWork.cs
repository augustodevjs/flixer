namespace Flixer.Catalog.Domain.Contracts;

public interface IUnityOfWork
{
    Task<bool> Commit();
}