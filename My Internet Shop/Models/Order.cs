using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MyInternetShop.Models;

namespace MyInternetShop.Models
{
    public class Order
    {
        public Order()
        {
            OrderEntities = new List<OrderEntity>();
        }

        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public string Status { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Cost { get; set; }

        public virtual User User { get; set; }

        public ICollection<OrderEntity> OrderEntities { get; set; }
    }
}
