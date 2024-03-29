﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Zone")]
    public class Zone
    {
        [Key]
        public int Zone_Id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Zone_Libelle { get; set; }
        public decimal Zone_Prix { get; set; }
    }
}
