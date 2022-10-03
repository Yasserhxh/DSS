using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    public class TypeChantierModel
    {
        public int Tc_Id { get; set; }
        public string Tc_Libelle { get; set; }
    }
}
