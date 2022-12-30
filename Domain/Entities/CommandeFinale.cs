using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
[Table("CommandeFinale")]
public class CommandeFinale
{
    [Key]
    public int CommandeId { get; set; }
    [ForeignKey("OffreDePrix")]
    public int? OffrePrixId { get; set; }
    [ForeignKey("Statut")]
    public int? IdStatut { get; set; }
    public string Observations { get; set; }
    public string PresenceLabo { get; set; }
    public string HeureLivraison { get; set; }
    public decimal VolumePrevi { get; set; }
    public OffreDePrix OffreDePrix { get; set; }
    public Statut Statut { get; set; }
}