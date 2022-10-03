using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    [Table("Tarif_Pompe")]
    public class TarifPompeRef
    {
        [Key]
        public int Tpr_Id { get; set; }
        public string LongFleche_Libelle { get; set; }
        public decimal LongFleche_Prix { get; set; }
    }
}
