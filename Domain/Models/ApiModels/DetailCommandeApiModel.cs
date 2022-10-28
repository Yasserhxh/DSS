namespace Domain.Models.ApiModels;

public class DetailCommandeApiModel
{
    public int IdDetailCommande { get; set; }
    public string ArticleDesignation { get; set; }
    public decimal? Montant { get; set; }
    public decimal? MontantRef { get; set; }
    public DateTime? DateProduction { get; set; }
    public decimal? Volume { get; set; }
    public string UniteLibelle { get; set; }
    public string ArticleFile { get; set; }

}