namespace Domain.Models;

public class OffreDePrix_DetailsModel
{
    public int IdDetailOffre { get; set; }
    public int? IdOffre { get; set; }
    public decimal? MontantRef { get; set; }
    public string ArticleFile { get; set; }
    public int? IdArticle { get; set; }
    public string CodeArticleSap { get; set; }
    public decimal? Montant { get; set; }
    public decimal? Volume { get; set; }
    public int? Unite_Id { get; set; }
    public string ArticleDescription { get; set; }
    public string ArticleName { get; set; }
    public OffreDePrixModel OffreDePrix { get; set; }
    public ArticleModel Article { get; set; }
    public UniteModel Unite { get; set; }
}