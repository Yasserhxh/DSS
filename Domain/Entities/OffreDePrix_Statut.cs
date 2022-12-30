using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
[Table("OffreDePrix_Statut")]
public class OffreDePrix_Statut
{
    [Key]
    public int OffreDePrix_StatutId { get; set; }
    [ForeignKey("OffreDePrix")]
    public int? OffreId { get; set; }
    [ForeignKey("Statut")]
    public int StatutId { get; set; }
    public OffreDePrix OffreDePrix { get; set; }
    public Statut Statut { get; set; }
}