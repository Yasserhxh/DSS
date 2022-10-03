using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Article")]
    public class Article
    {
        [Key]
        public int Article_Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Designation { get; set; }
        public double? Tarif { get; set; }
    }
}
