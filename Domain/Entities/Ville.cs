using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Ville")]
    public class Ville
    {
        [Key]
        public int IdVille { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string CodeVilleSap { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string NomVille { get; set; }
        [ForeignKey("Pays")]
        public int? IdPays { get; set; }

        public Pays Pays { get; set; }
        public string CodePaysSap { get; set; }
    }
}
