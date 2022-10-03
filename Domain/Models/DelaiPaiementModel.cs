using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    public class DelaiPaiementModel
    {
        public int Delai_Id { get; set; }
        public string Delai_Libelle { get; set; }
    }
}
