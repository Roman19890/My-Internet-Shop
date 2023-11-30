using Microsoft.AspNetCore.Identity;
using MyInternetShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyInternetShop.Models
{
    public class User : IdentityUser
    {
        public User() : base()
        {
            Orders = new List<Order>();
        }

        public int Year { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
