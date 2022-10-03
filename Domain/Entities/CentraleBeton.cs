using Domain.Authentication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    [Table("Centrale_Beton")]
    public class CentraleBeton
    {
        [Key]
        public int Ctr_Id { get; set; }
        [Column(TypeName = "nvarchar(80)")]
        public string Ctr_Nom { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Ctr_Adresse { get; set; }
        public int? Ctr_CodePostal { get; set; }
        [ForeignKey("VILLE")]
        public int? Ctr_Ville_Id { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string Ctr_Gsm { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Ctr_Email { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Ctr_Responsable { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string Ctr_Responsable_Gsm { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string Ctr_Latiture { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string Ctr_Longitude { get; set; }
        public decimal? Rayon { get; set; }

        public Ville VILLE { get; set; }
    }
}
