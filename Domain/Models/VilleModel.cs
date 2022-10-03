using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class VilleModel
    {
        public int IdVille { get; set; }
        public string CodeVilleSap { get; set; }
        public string NomVille { get; set; }
        public int? IdPays { get; set; }

        public PaysModel Pays { get; set; }
        public string CodePaysSap { get; set; }
    }
}
