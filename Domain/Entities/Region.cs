using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Region
    {
        [Key]
        public int Region_Id { get; set; }
        public string Region_Libelle { get; set; }
    }
}
