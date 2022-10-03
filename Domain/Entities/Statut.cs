using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    [Table("Statut")]
    public class Statut
    {
        [Key]
        public int IdStatut { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string CodeStatutSap { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Libelle { get; set; }
    }
}
