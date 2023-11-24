using Domain.Models;
using Domain.ViewModels;
using RepoAndService.Common;
using RepoAndService.Repository;
using RepoAndService.Services.Custom.UserTypeService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepoAndService.Services.Custom.SupplierServices
{
    public class SupplierServices : ISupplierService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IUserTypeService _userTypeService;

        public SupplierServices(IRepository<User> userRepository, IUserTypeService userTypeService)
        {
            _userRepository = userRepository;
            _userTypeService = userTypeService;
        }

        public async Task<bool> Delete(Guid id)
        {
            if(id != Guid.Empty)
            {
                User Usersupplier = await _userRepository.Get(id);
                if(Usersupplier != null)
                {
                    var res = _userRepository.Delete(Usersupplier);
                    return true;
                }
                else
                return false;
            }
            else
            return false;
        }

        public async Task<User> Find(Expression<Func<User, bool>> match)
        {
            return await _userRepository.Find(match);
        }

        public async Task<UserViewModel> Get(Guid id)
        {
            var supplier = await _userRepository.Get(id);
            var usertype = await _userTypeService.Find(x => x.TypeName == "Supplier");
            if (supplier == null)
            {
                return null;
            }
            else
            {
                if(supplier.UsertypeId == usertype.Id)
                {
                    UserViewModel userViewModel = new UserViewModel()
                    {
                       Id = supplier.Id,
                       UserCode = supplier.UserCode,
                       UserName = supplier.UserName,
                       Email = supplier.Email,
                       Password = supplier.Password,
                       PhoneNo = supplier.Phoneno,
                       Image = supplier.Image
                    };
                    UserTypeViewModel userTypeViewModel = new UserTypeViewModel();
                    if(usertype != null)
                    {
                        userTypeViewModel.Id = usertype.Id;
                        userTypeViewModel.TypeName = usertype.TypeName;
                        userViewModel.UserTypeViewModels.Add(userTypeViewModel);
                    }
                    return userViewModel;
                }
                return null;
            }
        }

        public async Task<ICollection<UserViewModel>> GetAll()
        {
            var usertype = await _userTypeService.Find(x => x.TypeName == "Supplier");
            ICollection<UserViewModel> supplierviewmodel = new List<UserViewModel>();

            ICollection<User> result = await _userRepository.FindAll(x=>x.UsertypeId == usertype.Id);
            foreach(User supplier in result)
            {
                UserViewModel supplierview = new UserViewModel()
                {
                    Id = supplier.Id,
                    UserCode = supplier.UserCode,
                    UserName = supplier.UserName,
                    Email = supplier.Email,
                    Password = Encryptor.DecryptString(supplier.Password),
                    PhoneNo = supplier.Phoneno,
                    Image = supplier.Image
                };
                UserTypeViewModel userView = new();
                if (usertype != null)
                {
                    userView.Id = usertype.Id;
                    userView.TypeName = usertype.TypeName;
                    supplierview.UserTypeViewModels.Add(userView);
                }
                supplierviewmodel.Add(supplierview);
            }
            if (result == null)
                return null;
            return supplierviewmodel;
        }

        public  User GetLast()
        {
            return _userRepository.GetLast();
        }

        public async Task<bool> Insert(UserInsertModel model, string photo)
        {
            var usertype = await _userTypeService.Find(x => x.TypeName == "Supplier");
            if (usertype != null)
            {
                User supplier = new()
                {
                    UserCode = model.UserCode,
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = Encryptor.EncryptString(model.Password),
                    Phoneno = model.PhoneNo,
                    UsertypeId = usertype.Id,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = model.IsActive,
                    Image = photo
                };
                var result = await _userRepository.Insert(supplier);
                return result;
            }
            else
                return false;
        }

        public async Task<bool> Update(UserUpdateModel model, string photo)
        {
            User supplier = await _userRepository.Get(model.Id);
            if (supplier != null)
            {
                supplier.UserCode = model.UserCode;
                supplier.UserName = model.UserName;
                supplier.Email = model.Email;
                supplier.Password = Encryptor.EncryptString(model.Password);
                supplier.Phoneno = model.PhoneNo;
                supplier.UsertypeId = supplier.UsertypeId;
                supplier.CreatedOn = DateTime.Now;
                supplier.UpdatedOn = DateTime.Now;
                supplier.IsActive = model.IsActive;
                if (photo == " ")
                    supplier.Image = supplier.Image;
                else
                    supplier.Image = photo;

                var result = await _userRepository.Update(supplier);
                return result;
            }
            else
                return false;
        }
    }
}
