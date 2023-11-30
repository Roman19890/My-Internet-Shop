using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyInternetShop.ViewModels
{
    public class AddCategoryViewModel
    {
        [Required(ErrorMessage = "Поле не может быть пустым")]
        [Display(Name = "Название категории")]
        [MinLength(5, ErrorMessage ="Минимум 5 символов")]
        public string CategoryName { get; set; }
    }
}
