using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class UserTypeViewModel
    {
        public Guid Id { get; set; }
        public string TypeName { get; set; }
    }
    public class UserTypeInsertModel
    {
        [Required(ErrorMessage = "User Type is required...!")]
        [StringLength(10)]
        public string TypeName { get; set; }
    }
        public class UserTypeUpdateModel : UserTypeInsertModel
        {
            [Required(ErrorMessage = "Id is neccessory for updation...!")]

            public Guid Id { get; set; }
        }
}
