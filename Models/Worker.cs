using System.ComponentModel.DataAnnotations;

namespace SklepMVC.Models
{
    public class Worker
    {

        public int id_worker_login  { get; set; }

        public int id_worker { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany!")]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane!")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        public Worker()
        {

        }
    }
}
