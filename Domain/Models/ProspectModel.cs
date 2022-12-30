namespace Domain.Models;

public class ProspectModel
{
    public int IdProspect { get; set; }
    public int? IdClient { get; set; }
    public int? IdChantier { get; set; }
    public string CodeClientSap { get; set; }
    public DateTime DateProspect { get; set; }
    public string Commercial { get; set; }
    public bool? CheckOffre { get; set; }
    public ClientModel Client { get; set; }
    public ChantierModel Chantier { get; set; }
}