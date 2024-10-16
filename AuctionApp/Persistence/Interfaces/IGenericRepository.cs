using System.Linq.Expressions;

namespace AuctionApp.Persistence.Interfaces;

public interface IGenericRepository<T>  where T : class 
{
    void Add(T entity);
    void Delete(T entity);
    void Update(T entity);
    void Save();
    T GetById(Guid id);
    List<T> GetAll();
    List<T> FindBy(Expression<Func<T, bool>> predicate);
}