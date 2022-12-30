namespace Domain.Models;

public class CommandeFinale_DetailsModel
{
    public int Id_DetailCommandeFinale { get; set; }
    public int? IdCommandeFinale { get; set; }
    public decimal MontantRef { get; set; }
    public string ArticleFile { get; set; }
    public int? IdArticle { get; set; }
    public string CodeArticleSap { get; set; }
    public decimal Montant { get; set; }
    public decimal Volume { get; set; }
    public int? Unite_Id { get; set; }
    public CommandeFinaleModel CommandeFinale { get; set; }
    public ArticleModel Article { get; set; }
    public UniteModel Unite { get; set; }
}