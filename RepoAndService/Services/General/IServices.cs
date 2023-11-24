using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepoAndService.Services.General
{
    public interface IServices<T>
    {
        Task<ICollection<T>> GetAll();
        Task<T> Get(Guid id);
        Task<bool> Insert(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);

        Task<T?> Find(Expression<Func<T, bool>> match);
        Task<ICollection<T>>FindAll(Expression<Func<T, bool>> match);
    }
}
