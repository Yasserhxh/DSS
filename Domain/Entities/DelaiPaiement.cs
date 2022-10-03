using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    [Table("Delai_Paiement")]
    public class DelaiPaiement
    {
        [Key]
        public int Delai_Id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Delai_Libelle { get; set; }
    }
}
