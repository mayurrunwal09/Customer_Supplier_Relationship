using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ItemImages : BaseEntity
    {
        [Required(ErrorMessage ="Enter Item ID")]
        public Guid ItemId { get; set; }

        public string ItemImage { get; set; }

        [JsonIgnore]
        public virtual Item Item { get; set; }
    }
}
