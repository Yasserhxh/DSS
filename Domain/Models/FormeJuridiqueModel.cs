using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    public class FormeJuridiqueModel
    {
        public int FormeJuridique_Id { get; set; }
        public string FormeJuridique_Libelle { get; set; }
    }
}
