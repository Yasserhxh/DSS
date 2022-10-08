using Domain.Models;
using Domain.Models.ApiModels;
using Domain.Models.Commande;

namespace Service.IServices
{
    public interface ICommandeService
    {
        Task<List<FormeJuridiqueModel>> GetFormeJuridiques();
        Task<List<TypeChantierModel>> GetTypeChantiers();
        Task<List<ZoneModel>> GetZones();
        Task<List<ArticleModel>> GetArticles();
        Task<List<DelaiPaiementModel>> GetDelaiPaiements();
        Task<List<CentraleBetonModel>> GetCentraleBetons();
        Task<double> GetTarifArticle(int Id);
        Task<bool> CreateCommande(CommandeViewModel commandeViewModel);
        Task<List<ClientModel>> GetClients(string Ice = null, string Cnie = null, string RS = null);
        Task<List<CommandeApiModel>> GetCommandes(int ClientId, DateTime? DateCommande);
        Task<CommandeModel> GetCommande(int? id);
        Task<List<TarifPompeRefModel>> GetTarifPompeRefs();
        public Task<List<VilleModel>> GetVilles();
        public Task<List<PaysModel>> GetPays();
        Task<double> GetTarifZone(int Id);
        Task<double> GetTarifPompe(int Id);
        Task<List<CommandeApiModel>> GetCommandesPT(int? ClientId, DateTime? DateCommande);
        Task<List<DetailCommandeApiModel>> GetCommandesDetails(int? commandeId);
        Task<bool> ProposerPrix(int Id, decimal Tarif, string UserName);
        Task<List<CommandeApiModel>> GetCommandesDAPBE(int? ClientId, DateTime? DateCommande);
        Task<List<CommandeApiModel>> GetCommandesRC(int? ClientId, DateTime? DateCommande);
        Task<List<CommandeApiModel>> GetCommandesCV(int? ClientId, DateTime? DateCommande);
        Task<bool> ProposerPrixDABPE(int Id, decimal Tarif);
        Task<bool> RefuserCommande(int Id, string Commentaire, string UserName);
        Task<bool> ValiderCommande(int Id, string Commentaire, string UserName);
        Task<List<CommandeApiModel>> GetCommandesRL(int? ClientId, DateTime? DateCommande);
        Task<bool> UpdateCommande(int id, CommandeViewModel commandeViewModel, string UserName);
        Task<bool> FixationPrixTransport(int Id, double VenteT, double VenteP);
    }
}
