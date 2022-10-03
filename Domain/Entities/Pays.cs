using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Pays")]
    public class Pays
    {
        [Key]
        public int IdPays { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string CodePaysSap { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string NomPays { get; set; }
    }
}
