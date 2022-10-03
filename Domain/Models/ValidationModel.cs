using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    public class ValidationModel
    {
        public int Validation_Id { get; set; }
        public string ValidationLibelle { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Fonction { get; set; }
        public int IdCommande { get; set; }
        public int? IdStatut { get; set; }
        public string UserId { get; set; }
        public DateTime? Date { get; set; }
        public CommandeModel Commande { get; set; }
    }
}
