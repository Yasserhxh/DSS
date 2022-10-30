namespace Domain.Models.Commande;

public class JsonBetonModifApi
{
    public string Useremail { get; set; }
    
    public List<CommandeModifVenteApi> CommandeModifVenteApis { get; set; } 
}