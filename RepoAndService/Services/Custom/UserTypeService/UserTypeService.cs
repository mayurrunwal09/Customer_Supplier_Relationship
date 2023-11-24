using Domain.Models;
using Domain.ViewModels;
using RepoAndService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepoAndService.Services.Custom.UserTypeService
{
    public class UserTypeService : IUserTypeService
    {

        private readonly IRepository<UserType> _userType;

        public UserTypeService(IRepository<UserType> userType)
        {
            _userType = userType;
        }

        public async Task<ICollection<UserTypeViewModel>> GetAll()
        {
            ICollection<UserTypeViewModel> userTypeViewModels = new List<UserTypeViewModel>();
            ICollection<UserType> userTypes = await _userType.GetAll();
            foreach (UserType userType in userTypes)
            {
                UserTypeViewModel userTypeView = new()
                {
                    Id = userType.Id,
                    TypeName = userType.TypeName
                };
                userTypeViewModels.Add(userTypeView);
            }
            return userTypeViewModels;
        }

        public async Task<bool> Delete(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                UserType userType = await _userType.Get(Id);
                if (userType != null)
                {
                    var user = _userType.Delete(userType);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public async Task<UserTypeViewModel> Get(Guid Id)
        {
            var result = await _userType.Get(Id);
            if (result == null)
            {
              return null;
            }
            else
            {
                UserTypeViewModel userTypeViewModel = new()
                {
                    Id = result.Id,
                    TypeName = result.TypeName
                };
                return userTypeViewModel;
            }
        }

        public UserType GetLast()
        {
            return _userType.GetLast();
        }

        public Task<bool> Insert(UserTypeInsertModel userInsertModel)
        {
                UserType userType = new()
                {
                    TypeName = userInsertModel.TypeName,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = true
                };
                return _userType.Insert(userType);
        }

        public async Task<bool> Update(UserTypeUpdateModel userUpdateModel)
        {
            UserType userType = await _userType.Get(userUpdateModel.Id);
            if (userType != null)
            {
                userType.TypeName = userUpdateModel.TypeName;
                userType.UpdatedOn = System.DateTime.Now;
                var result = await _userType.Update(userType);
                return result;
            }
            else
                return false;
        }
        public Task<UserType> Find(Expression<Func<UserType, bool>> match)
        {
            return _userType.Find(match);
        }
    }
}
