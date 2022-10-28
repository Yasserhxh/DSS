namespace Domain.Models.Commande;

public class CommandeModifApi
{
    public int CommandeId { get; set; }
    public double CommandeTarifTrans { get; set; }
    public double CommandeTarifPomp { get; set; }
    public int CommandeDetailId { get; set; }
    public string UserEmail { get; set; }
    public string CommandeBetonArticleFile { get; set; }

    public decimal CommandeTarifBeton { get; set; }
}