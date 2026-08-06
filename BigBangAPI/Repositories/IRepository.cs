using System.Linq.Expressions;

namespace BigBangAPI.Repositories
{
    public interface IRepository<T>
    {
        // Cuidado para não violar o princípio ISP
        IQueryable<T> GetAll();

        Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate);

        T Create(T entity);

        T Update(T entity);

        T Delete(T entity);
    }
}