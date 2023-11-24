using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CustomerItem : BaseEntity
    {
        [Required(ErrorMessage ="Enter UserID")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        [Required(ErrorMessage ="Enter ItemID")]
        public Guid ItemId { get; set; }
        public virtual Item Item { get; set; }  
    }
}
