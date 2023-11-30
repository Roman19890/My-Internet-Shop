using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyInternetShop.ViewModels
{
    public class RegisterViewModel: IValidatableObject
    {
        [Required(ErrorMessage = "Поле не может быть пустым")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        
        [Required(ErrorMessage = "Поле не может быть пустым")]
        [StringLength(20, ErrorMessage = "Поле {0} должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 3)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        [Display(Name = "Год рождения")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Поле {0} должно иметь минимум {2} символов", MinimumLength = 5)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (Year < 1875 || Year > DateTime.Now.Year)
            {
                errors.Add(new ValidationResult("Некорректное значение года",new List<string>() { "Year" }));
            }

            return errors;
        }
    }
}
