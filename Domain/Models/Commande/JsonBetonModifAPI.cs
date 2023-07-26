namespace Domain.Models.Commande;

public class JsonBetonModifApi
{
    public string Useremail { get; set; }
    public int IdCommande { get; set; }
    public string isBetonSpecial { get; set; }
    public List<CommandeModifVenteApi> CommandeModifVenteApis { get; set; } 
}