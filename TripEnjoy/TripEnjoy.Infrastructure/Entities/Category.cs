using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Infrastructure.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }        // category_id

        [Required]
        [StringLength(50)]                         // category_name - nvarchar(50)
        public string CategoryName { get; set; }
        public bool CategoryStatus { get; set; }   // category_Status - bit
     
    }
}
