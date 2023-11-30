using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyInternetShop.Models
{
    public class Category
    {
        public Category()
        {
            Products = new List<Product>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        [MinLength(5, ErrorMessage = "Минимум 5 символов")]
        public string CategoryName { get; set; }
        [Required]
        public virtual ICollection<Product> Products { get; set; }
    }
}
