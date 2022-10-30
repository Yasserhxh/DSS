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
        Task<List<CommandeApiModel>> GetCommandes(List<int>  ClientId, DateTime? DateCommande, string DateDebutSearch, string DateFinSearch);
        Task<CommandeModel> GetCommande(int? id);
        Task<List<TarifPompeRefModel>> GetTarifPompeRefs();
        public Task<List<VilleModel>> GetVilles();
        public Task<List<PaysModel>> GetPays();
        Task<double> GetTarifZone(int Id);
        Task<double> GetTarifPompe(int Id);
        Task<List<CommandeApiModel>> GetCommandesPT(List<int>  ClientId, DateTime? DateCommande, string DateDebutSearch, string DateFinSearch);
        Task<List<DetailCommandeApiModel>> GetCommandesDetails(int? commandeId);
        Task<List<StatutModel>> GetCommandesStatuts(int? id);
        Task<bool> ProposerPrix(int Id, decimal Tarif, string UserName, string articleFile);
        Task<List<CommandeApiModel>> GetCommandesDAPBE(List<int>  ClientId, DateTime? DateCommande, string dateDebutSearch, string dateFinSearch);
        Task<List<CommandeApiModel>> GetCommandesRC(List<int>  ClientId, DateTime? DateCommande, string dateDebutSearch, string dateFinSearch);
        Task<List<CommandeApiModel>> GetCommandesCV(List<int>  ClientId, DateTime? DateCommande, string dateDebutSearch, string dateFinSearch);
        Task<bool> ProposerPrixDABPE(int Id, decimal Tarif);
        Task<bool> RefuserCommande(int Id, string Commentaire, string UserName);
        Task<bool> ValiderCommande(int Id, string Commentaire, string UserName);
        Task<List<CommandeApiModel>> GetCommandesRL(List<int> ClientIds, DateTime? DateCommande, string DateDebutSearch, string DateFinSearch);
        Task<bool> UpdateCommande(int id, CommandeViewModel commandeViewModel, string UserName);
        Task<bool> FixationPrixTransport(int Id, double VenteT, double VenteP, string email);
        ClientModel FindFormulaireClient(string Ice, string Cnie);
        Task<List<ValidationModel>> GetListValidation(int commandeId);
        Task<bool> SetCommande(int commandeId);
        Task<List<CommandeApiModel>> GetCommandesValide(List<int> clientId, DateTime? dateTime, string dateDebutSearch,
            string dateFinSearch);

        Task<bool> FixationPrixRC(List<CommandeModifVenteApi> commandeModifApi, string UserEmail, int IdCommande);
    }
}
