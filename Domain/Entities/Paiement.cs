using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Paiement")]
    public class Paiement
    {
        [Key]
        public int Paiement_Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Conditions { get; set; }
    }
}
