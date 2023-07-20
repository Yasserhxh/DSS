using Microsoft.EntityFrameworkCore.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Commande")]
    public class Commande
    {
        public Commande()
        {
            CommandeStatuts = new List<CommandeStatut>();
        }
        [Key]
        public int IdCommande { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string CodeCommandeSap { get; set; }
        [ForeignKey("Client")]
        public int? IdClient { get; set; }
        [ForeignKey("Chantier")]
        public int? IdChantier { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string CodeClientSap { get; set; }
        public string Currency { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? DateCommande { get; set; }
        public decimal? MontantCommande { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? DateLivraisonSouhaite { get; set; }
        [ForeignKey("Statut")]
        public int? IdStatut { get; set; }
        [ForeignKey("Tarif_Pompe")]
        public int? LongFleche_Id { get; set; }
        public double TarifAchatTransport { get; set; }
        public double TarifAchatPompage { get; set; }
        public double TarifVenteTransport { get; set; }
        public double TarifVentePompage { get; set; }
        public string Conditions { get; set; }
        public string Delai_Paiement { get; set; }
        public string Commentaire { get; set; }
        public string ArticleFile { get; set; }
        public bool IsProspection { get; set; }
        public string PresenceLabo { get; set; }
        public string RegimeTaxe { get; set; }
        public string LaboDeControle { get; set; }
        public bool? FicheIsGenerated { get; set; } = false;
        public decimal? VolumePompe { get; set; }

        //public string Commercial_ID { get; set; }
        public Client Client { get; set; }
        public Statut Statut { get; set; }
        public Chantier Chantier { get; set; }
        public List<DetailCommande> DetailCommandes { get; set; }
        public List<CommandeStatut> CommandeStatuts { get; set; }
        public TarifPompeRef Tarif_Pompe { get; set; }
    }
}
