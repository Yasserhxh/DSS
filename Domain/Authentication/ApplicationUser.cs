using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

#nullable disable

namespace Domain.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "nvarchar(256)")]
        public string Nom { get; set; }
        [Column(TypeName = "nvarchar(256)")]
        public string Prenom { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("Region")]
        public int? VilleId { get; set; }
        public Region Region { get; set; }
    }
}
