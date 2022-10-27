namespace Domain.Models
{
    public class CommandeModel
    {
        public CommandeModel()
        {
            CommandeStatuts = new List<CommandeStatutModel>();

        }
        public int IdCommande { get; set; }
        public string CodeCommandeSap { get; set; }
        public int? IdClient { get; set; }
        public int? IdChantier { get; set; }
        public string CodeClientSap { get; set; }
        public string Currency { get; set; }
        public DateTime? DateCommande { get; set; }
        public decimal? MontantCommande { get; set; }
        public DateTime? DateLivraisonSouhaite { get; set; }
        public int? IdStatut { get; set; }
        public double TarifAchatTransport { get; set; }
        public double TarifAchatPompage { get; set; }
        public double TarifVenteTransport { get; set; }
        public double TarifVentePompage { get; set; }
        public string Conditions { get; set; }
        public string Delai_Paiement { get; set; }
        public int? LongFleche_Id { get; set; }
        public string Commentaire { get; set; }
        public string ArticleFile { get; set; }
        public bool IsProspection { get; set; }
        public ClientModel Client { get; set; }
        public StatutModel Statut { get; set; }
        public ChantierModel Chantier { get; set; }
        public List<CommandeStatutModel> CommandeStatuts { get; set; }
        public List<DetailCommandeModel> DetailCommandes { get; set; }
        public TarifPompeRefModel Tarif_Pompe { get; set; }
    }
}
