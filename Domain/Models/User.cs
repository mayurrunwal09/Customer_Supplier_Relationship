using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User:BaseEntity
    {
   
        public string UserCode { get; set; }


        [Required(ErrorMessage = "Please Enter UserName...!")]
        [StringLength(100)]
        public string UserName { get; set; }

        [RegularExpression(@"/\S+@\S+\.\S/", ErrorMessage = "Enter Valid Email...!")]
        public string Email { get;set; }
        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        public string Phoneno { get; set; }
        public string Image { get;set; }

        [Required(ErrorMessage ="Please select UserType")]
        public Guid UsertypeId { get; set; }

        [JsonIgnore]
        public virtual UserType UserType { get; set; }
        public virtual List<ItemSupplier> ItemSuppliers { get; set; }
        public virtual List<CustomerItem> CustomerItems { get; set; }
    }
}
