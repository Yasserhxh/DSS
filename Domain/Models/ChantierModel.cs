using Domain.Authentication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    public class ChantierModel
    {
        public int Ctn_Id { get; set; }
        public string Ctn_Nom { get; set; }
        public string MaitreOuvrage { get; set; }
        public decimal VolumePrevisonnel { get; set; }
        public decimal Duree { get; set; }
        public decimal Rayon { get; set; }
        public int Ctn_Zone_Id { get; set; }
        public int Ctn_Tc_Id { get; set; }
        public int Ctn_Ctr_Id { get; set; }
        public string Ctn_Adresse { get; set; }
        public string Ctn_Latiture { get; set; }
        public string Ctn_Longitude { get; set; }

        public ZoneModel ZONE_CHANTIER { get; set; }
        public TypeChantierModel Type_Chantier { get; set; }
        public CentraleBetonModel CentraleBeton { get; set; }

    }
}
