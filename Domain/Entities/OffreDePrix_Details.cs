using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
[Table("OffreDePrix_Details")]
public class OffreDePrix_Details
{
    [Key]
    public int IdDetailOffre { get; set; }
    [ForeignKey("OffreDePrix")]
    public int? IdOffre { get; set; }
    public decimal? MontantRef { get; set; }
    public string ArticleFile { get; set; }
    public string ArticleDescription { get; set; }
    public string ArticleName { get; set; }
    [ForeignKey("Article")]
    public int? IdArticle { get; set; }
    public string CodeArticleSap { get; set; }
    public decimal? Montant { get; set; }
    public decimal? Volume { get; set; }
    [ForeignKey("Unite")]
    public int? Unite_Id { get; set; }
    public OffreDePrix OffreDePrix { get; set; }
    public Article Article { get; set; }
    public Unite Unite { get; set; }
}