using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
#nullable disable

namespace Domain.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "nvarchar(256)")]
        public string Nom { get; set; }
        [Column(TypeName = "nvarchar(256)")]
        public string Prenom { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
