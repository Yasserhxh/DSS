using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Nom is required")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Prenom is required")]
        public string Prenom { get; set; }

        public string Fonction { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Telephone is required")]
        public string PhoneNumber { get; set; }
    }
}
