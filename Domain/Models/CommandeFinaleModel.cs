namespace Domain.Models;

public class CommandeFinaleModel
{
    public int CommandeId { get; set; }
    public int? OffrePrixId { get; set; }
    public int? IdStatut { get; set; }
    public string Observations { get; set; }
    public string PresenceLabo { get; set; }
    public string HeureLivraison { get; set; }
    public decimal VolumePrevi { get; set; }
    public OffreDePrixModel OffreDePrix { get; set; }
    public StatutModel Statut { get; set; }
}