namespace Domain.Models;

public class OffreDePrix_StatutModel
{
    public int OffreDePrix_StatutId { get; set; }
    public int? OffreId { get; set; }
    public int StatutId { get; set; }
    public OffreDePrixModel OffreDePrix { get; set; }
    public StatutModel Statut { get; set; }
}