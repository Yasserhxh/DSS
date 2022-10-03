using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Forme_Juridique")]
    public class FormeJuridique
    {
        [Key]
        public int FormeJuridique_Id { get; set; }
        [Column(TypeName = "nvarchar(150)")]
        public string FormeJuridique_Libelle { get; set; }
    }
}
