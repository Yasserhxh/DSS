using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Type_Chantier")]
    public class TypeChantier
    {
        [Key]
        public int Tc_Id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Tc_Libelle { get; set; }
    }
}
