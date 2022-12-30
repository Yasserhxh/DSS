namespace Domain.Models
{
    public class DetailCommandeModel
    {
        public int IdDetailCommande { get; set; }
        public int? IdCommande { get; set; }
        public string CodeCommandeSap { get; set; }
        public int? IdArticle { get; set; }
        public string CodeArticleSap { get; set; }
        public string ArticleFile { get; set; }
        public decimal? MontantRef { get; set; }
        public string ArticleDescription { get; set; }
        public string ArticleName { get; set; }
        public decimal? Montant { get; set; }
        public DateTime? DateProduction { get; set; }
        public int? IdStatut { get; set; }
        public int Unite_Id { get; set; }
        public decimal? Volume { get; set; }
        public CommandeModel Commande { get; set; }
        public ArticleModel Article { get; set; }
        public StatutModel Statut { get; set; }
    }
}
