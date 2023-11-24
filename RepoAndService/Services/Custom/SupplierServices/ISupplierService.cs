using Domain.Models;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepoAndService.Services.Custom.SupplierServices
{
    public interface ISupplierService
    {
      Task<ICollection<UserViewModel>>GetAll();
      Task<UserViewModel>Get(Guid id);
        User GetLast();
        Task<bool> Insert(UserInsertModel model,string photo);
        Task<bool> Update(UserUpdateModel model,string photo);
        Task<bool> Delete(Guid id);
        Task<User>Find(Expression<Func<User, bool>> match);
    }
}
