using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
[Table("CommandeStatut")]
public class CommandeStatut
{
    [Key]
    public int CommandeStatutId { get; set; }
    [ForeignKey("Commande")]
    public int CommandeId { get; set; }
    [ForeignKey("Statut")]
    public int StatutId { get; set; }
    public Commande Commande { get; set; }
    public Statut Statut { get; set; }
}