namespace Flixer.Catalog.Domain.Contracts;

public interface IUnitOfWork
{
    public Task<bool> Commit();
}