using System.ComponentModel.DataAnnotations;

namespace SQLWorker.Web.Models.Request
{
    public class LogInModel
    {
        [Required(ErrorMessage = "Потрібно вказати email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Потрібно вказати пароль")]
        public string Password { get; set; }
    }
}