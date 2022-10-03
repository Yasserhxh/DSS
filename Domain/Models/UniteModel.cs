using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class UniteModel
    {
        public int IdUnite { get; set; }
        public string Libelle { get; set; }
    }
}
