using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class PaysModel
    {
        public int IdPays { get; set; }
        public string CodePaysSap { get; set; }
        public string NomPays { get; set; }
    }
}
