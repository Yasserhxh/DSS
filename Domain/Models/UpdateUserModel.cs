using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public class UpdateUserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        public string Password { get; set; }
        [Required(ErrorMessage = "Nom is required")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Prenom is required")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Telephone is required")]
        public string PhoneNumber { get; set; }
    }
}
