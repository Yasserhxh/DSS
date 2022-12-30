using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
[Table("CommandeFinale_Details")]
public class CommandeFinale_Details
{
    [Key]
    public int Id_DetailCommandeFinale { get; set; }
    [ForeignKey("CommandeFinale")]
    public int? IdCommandeFinale { get; set; }
    public decimal MontantRef { get; set; }
    public string ArticleFile { get; set; }
    [ForeignKey("Article")]
    public int? IdArticle { get; set; }
    public string CodeArticleSap { get; set; }
    public decimal Montant { get; set; }
    public decimal Volume { get; set; }
    [ForeignKey("Unite")]
    public int? Unite_Id { get; set; }
    public CommandeFinale CommandeFinale { get; set; }
    public Article Article { get; set; }
    public Unite Unite { get; set; }
}