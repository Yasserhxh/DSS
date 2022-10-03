using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Validateur")]
    public class Validation
    {
        [Key]
        public int Validation_Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string ValidationLibelle { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Nom { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Prenom { get; set; }
        [Column(TypeName = "nvarchar(80)")]
        public string Fonction { get; set; }
        public string UserId { get; set; }
        [ForeignKey("Commande")]
        public int IdCommande { get; set; }
        public int? IdStatut { get; set; }
        public DateTime? Date { get; set; }
        public Commande Commande { get; set; }
    }
}
