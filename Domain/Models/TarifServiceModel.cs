using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    public class TarifServiceModel
    {
        public int TS_Id { get; set; }
        public double TarifTransport { get; set; }
        public double TarifPompage { get; set; }
    }
}
