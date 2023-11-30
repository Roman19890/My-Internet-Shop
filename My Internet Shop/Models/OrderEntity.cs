using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyInternetShop.Models
{
    public class OrderEntity
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [Key]
        public int OrderId { get; set; }

        public int Count { get; set; }

        public virtual Product Product { get; set; }
    }
}
