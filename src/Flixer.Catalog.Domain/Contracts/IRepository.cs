using System.Linq.Expressions;
using Flixer.Catalog.Domain.SeedWork;

namespace Flixer.Catalog.Domain.Contracts;

public interface IRepository<TAggregate> : IDisposable where TAggregate : AggregateRoot
{
    public IUnityOfWork UnityOfWork { get; }
    void Create(TAggregate entity);
    Task<TAggregate?> GetById(Guid? id);
    void Update(TAggregate entity);
    void Delete(TAggregate entity);
}