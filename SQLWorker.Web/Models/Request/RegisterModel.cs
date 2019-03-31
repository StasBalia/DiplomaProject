using System.ComponentModel.DataAnnotations;

namespace SQLWorker.Web.Models.Request
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Потрібно вказати ім'я")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Потрібно вказати e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Потрібно вказати пароль")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string RepeatPassword { get; set; }
    }
}