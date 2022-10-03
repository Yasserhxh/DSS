﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
