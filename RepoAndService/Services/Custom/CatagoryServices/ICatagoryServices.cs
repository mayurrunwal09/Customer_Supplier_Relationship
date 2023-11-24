using Domain.Models;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepoAndService.Services.Custom.CatagoryServices
{
    public interface ICatagoryServices
    {
        Task<ICollection<CatagoryViewModel>> GetAll();
        Task<CatagoryViewModel> Get(Guid Id);
        Catagory GetLast();
        Task<bool> Insert(CatagoryInsertModel categoryInsertModel);
        Task<bool> Update(CatagoryUpdateModel categoryUpdateModel);
        Task<bool> Delete(Guid Id);
        Task<Catagory> Find(Expression<Func<Catagory, bool>> match);
    }
}
