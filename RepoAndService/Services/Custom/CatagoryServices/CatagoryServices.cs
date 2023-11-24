using Domain.Models;
using Domain.ViewModels;
using RepoAndService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepoAndService.Services.Custom.CatagoryServices
{
    public class CatagoryServices : ICatagoryServices
    {
        private readonly IRepository<Catagory> _serviceCategory;
        public CatagoryServices(IRepository<Catagory> category)
        {
            _serviceCategory = category;
        }
        public async Task<bool> Delete(Guid Id)
        {
            if (Id != Guid.Empty)
            {
               Catagory catagory = await _serviceCategory.Get(Id);
                if (catagory != null)
                {
                    _ = _serviceCategory.Delete(catagory);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public Task<Catagory> Find(Expression<Func<Catagory, bool>> match)
        {
            return _serviceCategory.Find(match);
        }

        public async Task<CatagoryViewModel> Get(Guid Id)
        {
            var result = await _serviceCategory.Get(Id);
            if (result == null)
            {
                return null;
            }
            else
            {
                CatagoryViewModel categoryViewModel = new()
                {
                    Id = result.Id,
                    CatagoryName = result.CatagoryName
                };
                return categoryViewModel;
            }
        }

        public async Task<ICollection<CatagoryViewModel>> GetAll()
        {
            ICollection<CatagoryViewModel> categoryViewModel = new List<CatagoryViewModel>();
            ICollection<Catagory> result = await _serviceCategory.GetAll();
            foreach (Catagory category in result)
            {
                CatagoryViewModel categoryView = new()
                {
                    Id = category.Id,
                    CatagoryName = category.CatagoryName
                };
                categoryViewModel.Add(categoryView);
            }
            return categoryViewModel;
        }

        public Catagory GetLast()
        {
            return _serviceCategory.GetLast();
        }
        public Task<bool> Insert(CatagoryInsertModel categoryInsertModel)
        {
            Catagory category = new()
            {
                CatagoryName = categoryInsertModel.CatagoryName,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                IsActive = true
            };
            return _serviceCategory.Insert(category);
        }

        public async Task<bool> Update(CatagoryUpdateModel categoryUpdate)
        {
            Catagory catagory = await _serviceCategory.Get(categoryUpdate.Id);
            if (catagory != null)
            {
                catagory.CatagoryName = categoryUpdate.CatagoryName;
                catagory.UpdatedOn = System.DateTime.Now;
                var result = await _serviceCategory.Update(catagory);
                return result;
            }  
            else
                return false;
        }
    }
}
