using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Unite")]
    public class Unite
    {
        [Key]
        public int IdUnite { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Libelle { get; set; }
    }
}
