namespace Domain.Models;

public class OffreDePrixModel
{
    public OffreDePrixModel()
    {
        OffreStatuts = new List<OffreDePrix_StatutModel>();
    }
    public int OffreId { get; set; }
    public string Currency { get; set; }
    public DateTime DateOffre { get; set; }
    public decimal? MontantOffre { get; set; }
    public int? IdStatut { get; set; }
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
    public int? ProspectId { get; set; }
    public ProspectModel Prospect { get; set; }
    public StatutModel Statut { get; set; }
    public TarifPompeRefModel TarifPompeRef { get; set; }
    public List<OffreDePrix_StatutModel> OffreStatuts { get; set; }
    public List<OffreDePrix_DetailsModel> DetailOffre { get; set; }
}