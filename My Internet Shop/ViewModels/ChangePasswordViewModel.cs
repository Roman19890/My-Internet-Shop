using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyInternetShop.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        [StringLength(100, ErrorMessage = "Пароль должен содержать как минимум {2} символов", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
    }
}
