using Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class ItemViewModel
    {
        public Guid Id { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public List<CatagoryViewModel> Catagories { get; set; } = new List<CatagoryViewModel>();
        public List<ItemImagesView> ItemImages { get; set; } = new List<ItemImagesView>();
        public List<UserView> User { get; set; } = new List<UserView>();
    }
    public class ItemInsertModel
    {
       
        public string ItemCode { get; set; }

   
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Please Enter Item Description...!")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please Enter Item Price...!")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Enter Catagaory ID")]
        public Guid CatagoryId { get; set; }
        [Required(ErrorMessage = "Enter User ID")]
        public Guid UserId { get; set; }
        public IFormFile Images { get; set; }
        public bool? IsActive { get; set; }
    }
    public class ItemUpdateModel: ItemInsertModel
    {
        [Required(ErrorMessage = "Id is neccessory for updation...!")]
        public Guid Id { get; set; }
    }
    public class ItemImagesView
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string ItemImage { get; set; }
        public bool? IsActive { get; set; }
    }
    public class UserView
    {
        public Guid Id { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get;set; }
        public string UserImage { get; set; }
    }
    public class ExistingItemInsertModel
    {
        [Required(ErrorMessage = "Item Id is neccessory for insertion...!")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Please select User...!")]

        public Guid UserId { get; set; }
        public bool? IsActive { get; set; }
    }
}