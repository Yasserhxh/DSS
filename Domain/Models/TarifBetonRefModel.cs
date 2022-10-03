using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    public class TarifBetonRefModel
    {
        public int Tbr_Id { get; set; }
        public string Designation_Beton { get; set; }
        public double Tarif { get; set; }
    }
}
