using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities;

[Table("DetailCommandeV")]

public class DetailCommandeV
{ 
    [Key]
    public int IdDetailCommande { get; set; }
    [ForeignKey("CommandeV")]
    public int? IdCommande { get; set; }
    [Column(TypeName = "nvarchar(20)")]
    public string CodeCommandeSap { get; set; }
    public decimal? MontantRef { get; set; }
    public string ArticleFile { get; set; }
    [ForeignKey("Article")]
    public int? IdArticle { get; set; }
    [Column(TypeName = "nvarchar(20)")]
    public string CodeArticleSap { get; set; }
    public decimal? Montant { get; set; }
    public decimal? Volume { get; set; }
    public string ArticleDescription { get; set; }
    public string ArticleName { get; set; }
    public DateTime? DateProduction { get; set; }
    [ForeignKey("Unite")]
    public int Unite_Id { get; set; }

    public CommandeV CommandeV { get; set; }
    public Article Article { get; set; }
    public Unite Unite { get; set; }
}