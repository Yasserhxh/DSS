using Domain.Authentication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    public class PaiementModel
    {
        public int Paiement_Id { get; set; }
        public string Conditions { get; set; }
    }
}
