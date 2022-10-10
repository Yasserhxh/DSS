namespace Domain.Models;

public class CommandeStatutModel
{
    public int CommandeStatutId { get; set; }
    public int CommandeId { get; set; }
    public int StatutId { get; set; }
    public CommandeModel Commande { get; set; }
    public StatutModel Statut { get; set; }
    
}