using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using AuctionApp.Persistence.Interfaces;

namespace AuctionApp.Persistence.Repositories;

public abstract class GenericRepository<T> : IGenericRepository<T>
    where T : class
   
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public T GetById(Guid id)
    {
        return _dbSet.Find(id);
    }

    public List<T> GetAll()
    {
        return _dbSet.ToList();
    }

    public List<T> FindBy(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate).ToList();
    }
}