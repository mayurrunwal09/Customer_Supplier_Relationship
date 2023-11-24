using Domain.Models;
using Domain.ViewModels;
using Microsoft.Extensions.Options;
using RepoAndService.Common;
using RepoAndService.Repository;
using RepoAndService.Services.Custom.UserTypeService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepoAndService.Services.Custom.Customer_Service
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IUserTypeService _userTypeService;
        public CustomerService(IRepository<User> userRepository, IUserTypeService userTypeService)
        {
            _userRepository = userRepository;
            _userTypeService = userTypeService;
        }

        public async Task<bool> Delete(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                User customer = await _userRepository.Get(Id);
                if (customer != null)
                {
                    var res = _userRepository.Delete(customer);
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

        public async Task<UserViewModel> Get(Guid Id)
        {
            var result = await _userRepository.Get(Id);
            var typename = await _userTypeService.Find(x => x.TypeName == "Customer");
            if(result == null)
            {
                return null;
            }
            else
            {
                if(result.UsertypeId == typename.Id)
                {
                    UserViewModel userViewModel = new UserViewModel()
                    {
                        Id = result.Id,
                        UserCode = result.UserCode,
                        UserName = result.UserName,
                        Email = result.Email,
                        Password = result.Password,
                        PhoneNo = result.Phoneno,
                        Image = result.Image,
                        
                    };
                    UserTypeViewModel view = new();
                   
                   if(typename != null)
                    {
                        view.Id = typename.Id;
                        view.TypeName = typename.TypeName;
                        userViewModel.UserTypeViewModels.Add(view);
                    }
                    return userViewModel;
                   
                }
                return null;
            }
        }

        public async Task<ICollection<UserViewModel>> GetAll()
        {
            var usertype = await _userTypeService.Find(x => x.TypeName == "Customer");
            ICollection<UserViewModel> customerviewmodel = new List<UserViewModel>();

            ICollection<User> result = await _userRepository.FindAll(x=>x.UsertypeId == usertype.Id);
            foreach(User customer in result)
            {
                UserViewModel customerview = new()
                {
                    Id = customer.Id,
                    UserCode = customer.UserCode,
                    UserName = customer.UserName,
                    Email = customer.Email,
                    Password = Encryptor.DecryptString(customer.Password),
                    PhoneNo = customer.Phoneno,
                    Image = customer.Image,
                };
                UserTypeViewModel userview = new();
                if(usertype != null)
                {
                    userview.Id = usertype.Id;
                    userview.TypeName = usertype.TypeName;
                    customerview.UserTypeViewModels.Add(userview);
                }
                customerviewmodel.Add(customerview);
            }
            if(result == null)
            {
                return null;
            }
            return customerviewmodel;

        }

        public User GetLast()
        {
            return _userRepository.GetLast();
        }

        public async Task<bool> Insert(UserInsertModel userInsertModel, string photo)
        {
            var usertype = await _userTypeService.Find(x => x.TypeName == "Customer");
            if(usertype != null)
            {
                User customer = new()
                {
                    UserCode = userInsertModel.UserCode,
                    UserName = userInsertModel.UserName,
                    Email = userInsertModel.Email,
                    Password = Encryptor.EncryptString(userInsertModel.Password),
                    Phoneno = userInsertModel.PhoneNo,
                    UsertypeId = usertype.Id,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = userInsertModel.IsActive,
                    Image = photo

                };
                var result = await _userRepository.Insert(customer);
                return result;
            }
            else { return false; }
        }

        public async Task<bool> Update(UserUpdateModel userUpdateModel, string photo)
        {
            User customer = await _userRepository.Get(userUpdateModel.Id);
            if (customer != null)
            {
                customer.UserCode = userUpdateModel.UserCode;
                customer.UserName = userUpdateModel.UserName;
                customer.Email = userUpdateModel.Email;
                customer.Password = userUpdateModel.Password;
                customer.Phoneno = userUpdateModel.PhoneNo;
                customer.UsertypeId = customer.UsertypeId;
                customer.CreatedOn = DateTime.Now;
                customer.UpdatedOn = DateTime.Now;
                customer.IsActive = userUpdateModel.IsActive;
                if (photo == " ")
                {
                    customer.Image = customer.Image;
                }
                else
                    customer.Image = photo;
                var result = await _userRepository.Update(customer);
                return result;
            }
            else
              return  false;
        }
    }
}
