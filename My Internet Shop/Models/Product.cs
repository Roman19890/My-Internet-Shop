using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyInternetShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Category")]
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }

        [ForeignKey("Image")]
        public int? ImageId { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        [Display(Name = "Наименование товара")]
        [MinLength(5, ErrorMessage = "Минимум 5 символов")]
        public string ProductName { get; set; }

        [Display(Name = "Цена")]
        [Required(ErrorMessage = "Поле не может быть пустым")]
        public decimal? Price { get; set; }

        [Display(Name = "Количество")]
        [Required(ErrorMessage = "Поле не может быть пустым")]
        public int Quantity { get; set; }

        [Display(Name = "Описание и характеристики")]
        public string Description { get; set; }

        [Display(Name = "Категория")]
        public virtual Category Category { get; set; }
        public virtual Image image { get; set; }
    }
}
