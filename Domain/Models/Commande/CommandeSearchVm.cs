namespace Domain.Models.Commande
{
    public class CommandeSearchVm
    {
        public int? IdClient { get; set; }
        public DateTime? DateCommande { get; set; }
        public IEnumerable<CommandeModel> Commandes { get; set; }
        public IEnumerable<ClientModel> CLients { get; set; }
    }
}
