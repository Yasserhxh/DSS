using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    public class ArticleModel
    {
        public int Article_Id { get; set; }
        public string Designation { get; set; }
        public double? Tarif { get; set; }
    }
}
