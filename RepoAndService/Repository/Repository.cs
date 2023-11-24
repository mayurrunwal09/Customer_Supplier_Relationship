using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepoAndService.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly MainDBContext _dbContext;
        private readonly DbSet<T> entities;
        public Repository(MainDBContext dbContext)
        {
            _dbContext = dbContext;
            entities = _dbContext.Set<T>();
        }
        public Task<bool> Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<T> Find(Expression<Func<T, bool>> match)
        {
            return await entities.FirstOrDefaultAsync(match);
        }

        public async Task<ICollection<T>> FindAll(Expression<Func<T, bool>> match)
        {
           return await entities.Where(match).ToListAsync();
        }

        public async Task<T> Get(Guid Id)
        {
            return await entities.FindAsync(Id);
        }

        public async Task<ICollection<T>> GetAll()
        {
            return await entities.ToListAsync();
        }

        public  T GetLast()
        {
            if(entities.ToList()!= null)
            {
                return entities.ToList().LastOrDefault();
            }
            else
            {
                return entities.ToList().LastOrDefault();
            }
        } 

        public async Task<bool> Insert(T entity)
        {
            await entities.AddAsync(entity);
            var result = await _dbContext.SaveChangesAsync();
            if(result > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Update(T entity)
        {
            entities.Update(entity);
            var result = await _dbContext.SaveChangesAsync();
            if(result > 0)
            {
                return true;
            }
            return false;
        }
    }
}
