using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
[Table("OffreDePrix")]
public class OffreDePrix
{
    public OffreDePrix()
    {
        OffreStatuts = new List<OffreDePrix_Statut>();
    }
    [Key]
    public int OffreId { get; set; }
    public string Currency { get; set; }
    public DateTime DateOffre { get; set; }
    public decimal? MontantOffre { get; set; }
    [ForeignKey("Statut")]
    public int? IdStatut { get; set; }
    [ForeignKey("TarifPompeRef")]
    public int? LongFlech_Id { get; set; }
    public double TarifAchatTransport { get; set; }
    public double TarifAchatPompage { get; set; }
    public double TarifVenteTransport { get; set; }
    public double TarifVentePompage { get; set; }
    public string Prestation { get; set; }
    public string Conditions { get; set; }
    public string Delai_Paiement { get; set; }
    public string Commentaire { get; set; }
    public string ArticleFile { get; set; }
    public string ArticleDescription { get; set; }
    [ForeignKey("Prospect")]
    public int? ProspectId { get; set; }
    public Prospect Prospect { get; set; }
    public Statut Statut { get; set; }
    public TarifPompeRef TarifPompeRef { get; set; }
    public List<OffreDePrix_Statut> OffreStatuts { get; set; }
    public List<OffreDePrix_Details> DetailOffre { get; set; }
}