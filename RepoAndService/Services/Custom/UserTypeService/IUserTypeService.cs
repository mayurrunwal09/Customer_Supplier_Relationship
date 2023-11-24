using Domain.Models;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepoAndService.Services.Custom.UserTypeService
{
    public interface IUserTypeService
    {
        Task<ICollection<UserTypeViewModel>> GetAll();
        Task<UserTypeViewModel> Get(Guid Id);
        UserType GetLast();
        Task<bool> Insert(UserTypeInsertModel userInsertModel);
        Task<bool> Update(UserTypeUpdateModel userUpdateModel);
        Task<bool> Delete(Guid Id);
        Task<UserType> Find(Expression<Func<UserType, bool>> match);
    }
}
