﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Domain.SeedWork;
using Flixer.Catalog.Domain.Contracts;
using Flixer.Catalog.Infra.Data.EF.Context;

namespace Flixer.Catalog.Infra.Data.EF.Abstractions;

public abstract class Repository<TAggregate> : IRepository<TAggregate> where TAggregate: AggregateRoot
{
    private bool _isDisposed;
    private readonly DbSet<TAggregate> _dbSet;
    protected readonly FlixerCatalogDbContext Context;

    protected Repository(FlixerCatalogDbContext context)
    {
        Context = context;
        _dbSet = context.Set<TAggregate>();
    }
    
    public IUnityOfWork UnityOfWork => Context;
        
    public async Task<TAggregate?> FirstOrDefault(Expression<Func<TAggregate, bool>> expression)
    {
        return await _dbSet.AsNoTrackingWithIdentityResolution().Where(expression).FirstOrDefaultAsync();
    }
    
    public virtual async Task<List<TAggregate>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }
    
    public virtual async Task<TAggregate?> GetById(Guid? id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual void Create(TAggregate entity)
    {
        _dbSet.Add(entity);
    }

    public virtual void Update(TAggregate entity)
    {
        _dbSet.Update(entity);
    }

    public virtual void Delete(TAggregate entity)
    {
        _dbSet.Remove(entity);
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed) return;

        if (disposing) Context.Dispose();

        _isDisposed = true;
    }
}