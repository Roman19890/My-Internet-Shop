using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyInternetShop.ViewModels
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }

        public int ProductLeftOver { get; set; }
        public decimal? Sum { get; set; }

        public string ImageDataUrl { get; set; }
    }
}
