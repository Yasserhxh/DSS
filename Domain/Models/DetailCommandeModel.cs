using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class DetailCommandeModel
    {
        public int IdDetailCommande { get; set; }
        public int? IdCommande { get; set; }
        public string CodeCommandeSap { get; set; }
        public int? IdArticle { get; set; }
        public string CodeArticleSap { get; set; }
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
