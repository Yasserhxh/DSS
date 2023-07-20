namespace Domain.Models.ApiModels;

public class DetailCommandeApiModel
{
    public int IdDetailCommande { get; set; }
    public int IdCommande { get; set; }
    public int? IdArticle { get; set; }
    public string ArticleDesignation { get; set; }
    public string ArticleName { get; set; }
    public decimal? Montant { get; set; }
    public decimal? MontantRef { get; set; }
    public decimal? MargeBenef { get; set; }
    public decimal? MontantV1 { get; set; }
    public decimal? MontantRC { get; set; }
    public decimal? MontantDA { get; set; }
    public DateTime? DateProduction { get; set; }
    public decimal? Volume { get; set; }
    public string UniteLibelle { get; set; }
    public string ArticleFile { get; set; }

}