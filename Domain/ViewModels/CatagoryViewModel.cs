using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class CatagoryViewModel
    {
        public Guid Id { get; set; }
        public string CatagoryName { get; set; }
    }
    public class CatagoryInsertModel
    {
        [Required(ErrorMessage = "Please Enter Category Name...!")]
        [StringLength(50, ErrorMessage = "Category name can't exceed to 50 character...!")]
        public string CatagoryName { get; set; } 

    }
    public class CatagoryUpdateModel : CatagoryInsertModel
    {
        [Required(ErrorMessage = "Id is Required for Updating Category...!")]

        public Guid Id { get; set; }
    }
}
