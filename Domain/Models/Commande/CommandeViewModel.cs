
namespace Domain.Models.Commande
{
    public class CommandeViewModel
    {
        public CommandeModel Commande { get; set; }
        public CommandeVModel CommandeV { get; set; }
        public List<DetailCommandeModel> DetailCommandes { get; set; }
        public ClientModel Client { get; set; }
        public ChantierModel Chantier { get; set; }
        public int? ProspectionID { get; set; }
        //public IFormFile? file { get; set; }
    }
}
