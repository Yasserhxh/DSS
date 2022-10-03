using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    [Table("Forme_Juridique")]
    public class FormeJuridique
    {
        [Key]
        public int FormeJuridique_Id { get; set; }
        [Column(TypeName = "nvarchar(150)")]
        public string FormeJuridique_Libelle { get; set; }
    }
}
