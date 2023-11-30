using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyInternetShop.ViewModels
{
    public class EditUserViewModel: IValidatableObject
    {
        public string Id { get; set; }
        [EmailAddress(ErrorMessage = "Некорректный Email адрес")]
        public string Email { get; set; }

        [Display(Name ="Имя пользователя")]
        [MinLength(3, ErrorMessage = "Имя пользователя Минимум {1} символов")]
        public string UserName { get; set; }

        [Display(Name = "Год рождения")]
        public int Year { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (Year < 1875 || Year > DateTime.Now.Year)
            {
                errors.Add(new ValidationResult("Некорректное значение года", new List<string>() { "Year" }));
            }

            return errors;
        }
    }
}
