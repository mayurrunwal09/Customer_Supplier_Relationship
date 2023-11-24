using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Item : BaseEntity
    {
        
        public string ItemCode { get;set; }

        [Required(ErrorMessage ="Enter name of item")]
        [StringLength(100)]
        public string ItemName { get;set; }

        [Required(ErrorMessage = "Please Enter Item Description...!")]
        public string Description { get; set; }

        [Required(ErrorMessage ="Enter Price of item")]
        public double Price { get;set; }

        [Required(ErrorMessage ="Select catagory")]
        public Guid CatagoryId { get; set; }

        [JsonIgnore]
        public virtual List<ItemImages> ItemImages { get; set; }
        public virtual ItemSupplier ItemSupplier { get; set; }
        public virtual CustomerItem CustomerItem { get; set; }
        public virtual Catagory Catagory { get; set; }
    }
}
