using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Catagory : BaseEntity
    {
        [Required(ErrorMessage = "Please Enter Category Name...!")]
        [StringLength(50)]
        public string CatagoryName { get;set; }


        [JsonIgnore]
        public virtual List<Item> Items { get; set; }
    }
}
