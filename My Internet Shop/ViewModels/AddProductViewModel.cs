using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyInternetShop.ViewModels
{
    public class AddProductViewModel : IValidatableObject
    {
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }

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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (Quantity < 0)
            {
                errors.Add(new ValidationResult("Отрицательные значения недопустимы", new List<string>() { "Quantity" }));
            }

            return errors;
        }
    }
}
