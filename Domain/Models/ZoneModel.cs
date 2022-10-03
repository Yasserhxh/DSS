using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    public class ZoneModel
    {
        public int Zone_Id { get; set; }
        public string Zone_Libelle { get; set; }
        public decimal Zone_Prix { get; set; }
    }
}
