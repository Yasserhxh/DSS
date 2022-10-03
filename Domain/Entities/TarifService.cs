using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Tarif_Service")]
    public class TarifService
    {
        [Key]
        public int TS_Id { get; set; }
        public double TarifTransport { get; set; }
        public double TarifPompage { get; set; }
    }
}
