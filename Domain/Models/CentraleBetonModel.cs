using Domain.Authentication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    public class CentraleBetonModel
    {
        public int Ctr_Id { get; set; }
        public string Ctr_Nom { get; set; }
        public string Ctr_Adresse { get; set; }
        public int? Ctr_CodePostal { get; set; }
        public int? Ctr_Ville_Id { get; set; }
        public string Ctr_Gsm { get; set; }
        public string Ctr_Email { get; set; }
        public string Ctr_Responsable { get; set; }
        public string Ctr_Responsable_Gsm { get; set; }
        public string Ctr_Latiture { get; set; }
        public string Ctr_Longitude { get; set; }
        public decimal? Rayon { get; set; }

        public VilleModel VILLE { get; set; }
    }
}
