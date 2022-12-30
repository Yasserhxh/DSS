using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
[Table("Prospect")]
public class Prospect
{
    [Key]
    public int IdProspect { get; set; }
    [ForeignKey("Client")]
    public int? IdClient { get; set; }
    [ForeignKey("Chantier")]
    public int? IdChantier { get; set; }
    public string CodeClientSap { get; set; }
    public DateTime DateProspect { get; set; }
    public string Commercial { get; set; }
    public bool? CheckOffre { get; set; }
    public Client Client { get; set; }
    public Chantier Chantier { get; set; }
}