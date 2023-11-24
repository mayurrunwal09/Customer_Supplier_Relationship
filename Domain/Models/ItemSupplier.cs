using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ItemSupplier : BaseEntity
    {
        [Required(ErrorMessage ="Enter User ID")]
        public Guid UserId { get; set; }    
        public virtual User User { get; set; }

        [Required(ErrorMessage ="Enter Item ID")]
        public Guid ItemId { get; set; }
        public virtual Item Item { get; set; }
    }
}
