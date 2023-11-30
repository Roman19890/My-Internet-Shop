using System.ComponentModel.DataAnnotations;

namespace MyInternetShop.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Поле не может быть пустым")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}