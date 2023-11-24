using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class UserType : BaseEntity
    {
        [Required(ErrorMessage ="Select User Type")]
        [StringLength(100)]
        public string TypeName { get;set; }

        [JsonIgnore]
        public virtual List<User> Users { get; set; }
    }

}
