using Microsoft.EntityFrameworkCore;
using PetStoreApi.Helpers;
using System.Linq.Expressions;

namespace PetStoreApi.Services.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DataContext _context;
        public GenericRepository(DataContext context)
        {
            _context = context;
        }
        public IQueryable<T> FindAll()
        {
            return this._context.Set<T>().AsNoTracking();
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this._context.Set<T>()
                .Where(expression).AsNoTracking();
        }
        public void Create(T entity)
        {
            this._context.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            this._context.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            this._context.Set<T>().Remove(entity);
        }
    }
}
