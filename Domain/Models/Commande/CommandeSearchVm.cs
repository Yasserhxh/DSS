using Domain.Models.ApiModels;

namespace Domain.Models.Commande
{
    public class CommandeSearchVm
    {
        public List<int> IdClients { get; set; }
        public string UserRole { get; set; }
        public string IceClient { get; set; }
        public string CnieClient { get; set; }
        public string RsClient { get; set; }
        public DateTime? DateCommande { get; set; }
        public string DateDebutSearch { get; set; }
        public string DateFinSearch { get; set; }
        public IEnumerable<CommandeModel> Commandes { get; set; }
        public IEnumerable<CommandeApiModel> CommandesAPI { get; set; }
        public IEnumerable<ClientModel> CLients { get; set; }
    }
}
