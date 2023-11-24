using Domain;
using RepoAndService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepoAndService.Services.General
{
    public class Services<T> : IServices<T> where T : BaseEntity
    {
        private readonly IRepository<T> _repository;
        public Services(IRepository<T> repository)
        {
            _repository = repository;
        }
        public async Task<bool> Delete(T entity)
        {
            return await _repository.Delete(entity);
        }

        public async Task<T?> Find(Expression<Func<T, bool>> match)
        {
            return await _repository.Find(match);
        }

        public async Task<ICollection<T>> FindAll(Expression<Func<T, bool>> match)
        {
            return await _repository.FindAll(match);
        }

        public Task<T> Get(Guid id)
        {
            return _repository.Get(id);
        }

        public Task<ICollection<T>> GetAll()
        {
            return _repository.GetAll();
        }

        public async Task<bool> Insert(T entity)
        {
            return await _repository.Insert(entity);
        }

        public async Task<bool> Update(T entity)
        {
            return await _repository.Update(entity);
        }
    }
}
