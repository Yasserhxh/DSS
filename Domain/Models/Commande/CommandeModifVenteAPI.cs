namespace Domain.Models.Commande;

public class CommandeModifVenteApi
{
    public int IdCommande { get; set; }
    public decimal montant { get; set; }
    public decimal montantRef { get; set; }
    public int idDetailCommande { get; set; }
    public int volume { get; set; }
    public string articleDesignation { get; set; }
    public string articleFile { get; set; }
    public string uniteLibelle { get; set; }

  

}