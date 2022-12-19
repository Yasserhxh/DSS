namespace Domain.Models.ApiModels;

public class CommandeApiModel
{
    public int CommandeId { get; set; }
    public string CodeCommandeSap { get; set; }
    public string StatutCommande { get; set; }
    public DateTime? DateCommande { get; set; }
    public DateTime? DateLivraisonSouhaite { get; set; }
    public string HeureLivraisonSouhaite { get; set; }
    public double TarifAchatTransport { get; set; }
    public double TarifAchatPompage { get; set; }
    public double TarifVenteTransport { get; set; }
    public double TarifVentePompage { get; set; }
    public string Conditions { get; set; }
    public string DelaiPaiement { get; set; }
    public string LongFlecheLibelle { get; set; }
    public decimal LongFlechePrix { get; set; }
    public string Commentaire { get; set; }
    public string ArticleFile { get; set; }
    public string Ice { get; set; }
    public string Cnie { get; set; }
    public string FormeJuridique { get; set; }
    public string RaisonSociale { get; set; }
    public string Gsm { get; set; }
    public string Email { get; set; }
    public string Adresse { get; set; }
    public string DestinataireInterlocuteur { get; set; }
    public string Ville { get; set; }
    public string Pays { get; set; }
    public string CtnNom { get; set; }
    public string CtnZone { get; set; }
    public string CtnType { get; set; }
    public string MaitreOuvrage { get; set; }
    public decimal VolumePrevisonnel { get; set; }
    public decimal Duree { get; set; }
    public decimal Rayon { get; set; }
    public string CtrNom { get; set; }

    // public List<DetailCommandeApiModel> DetailsCommande { get; set; }
}