using System.Linq.Expressions;

namespace BigBangAPI.Repositories
{
    public interface IRepository<T>
    {
        // Cuidado para não violar o principio ISP
    IEnumerable<T> GetAll();
    T? GetById(Expression<Func<T, bool>> predicate);
    T Create(T entity);
    T? Update(T entity);
    T? Delete(T entity);
    }
}