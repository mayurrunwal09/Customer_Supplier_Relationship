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
    public class UserViewModel
    {
        public Guid Id { get; set; }    
        public string UserCode { get;set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Image { get; set; }

        public List<UserTypeViewModel> UserTypeViewModels { get; set; } = new List<UserTypeViewModel> ();

    }
    public class UserInsertModel
    {
        
        public string UserCode { get; set; }

       
        public string UserName { get; set; }

        
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        public string Address { get; set; }


        public string PhoneNo { get; set; }
        public IFormFile Image { get; set; }

        public bool? IsActive { get; set; }
    }
    public class UserUpdateModel : UserInsertModel
    {
        public Guid Id { get; set; }
    }
    public class LoginModel
    {
        [Required(ErrorMessage ="Enter UserName")]
        [StringLength(100)]
        public string UserName { get; set; }
        [Required]
        [StringLength(100)]
        public string Password { get; set; }    
    }
}
