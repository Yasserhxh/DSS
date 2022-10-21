using Microsoft.AspNetCore.Http;

namespace Domain.Models.Commande
{
    public class CommandeViewModel
    {
        public CommandeModel Commande { get; set; }
        public List<DetailCommandeModel> DetailCommandes { get; set; }
        public ClientModel Client { get; set; }
        public ChantierModel Chantier { get; set; }
         
        //public IFormFile? file { get; set; }
    }
}
