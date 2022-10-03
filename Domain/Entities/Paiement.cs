using Domain.Authentication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    [Table("Paiement")]
    public class Paiement
    {
        [Key]
        public int Paiement_Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Conditions { get; set; }
    }
}
