using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    public class StatutModel
    {
        public int IdStatut { get; set; }
        public string CodeStatutSap { get; set; }
        public string Libelle { get; set; }
    }
}
