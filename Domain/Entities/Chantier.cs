using Domain.Authentication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    [Table("Chantier")]
    public class Chantier
    {
        [Key]
        public int Ctn_Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Ctn_Nom { get; set; }
        public string MaitreOuvrage { get; set; }
        public decimal VolumePrevisonnel { get; set; }
        public decimal Duree { get; set; }
        public decimal Rayon { get; set; }
        [ForeignKey("ZONE_CHANTIER")]
        public int Ctn_Zone_Id { get; set; }
        [ForeignKey("Type_Chantier")]
        public int Ctn_Tc_Id { get; set; }
        [ForeignKey("Centrale_Beton")]
        public int Ctn_Ctr_Id { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string Ctn_Adresse { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string Ctn_Latiture { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string Ctn_Longitude { get; set; }

        public Zone ZONE_CHANTIER { get; set; }
        public TypeChantier Type_Chantier { get; set; }
        public CentraleBeton Centrale_Beton { get; set; }

    }
}
