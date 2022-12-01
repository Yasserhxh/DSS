using Domain.Entities;

namespace Repository.IRepositories
{
    public interface ICommandeRepository
    {
        Task<List<FormeJuridique>> GetFormeJuridiques();
        Task<List<TypeChantier>> GetTypeChantiers();
        Task<List<Zone>> GetZones();
        Task<List<Article>> GetArticles();
        Task<List<DelaiPaiement>> GetDelaiPaiements();
        Task<List<CentraleBeton>> GetCentraleBetons();
        Task<double> GetTarifArticle(int Id);
        Task<double> GetTarifZone(int Id);
        Task<int?> CreateChantier(Chantier chantier);
        Task<int?> CreateClient(Client client);
        Task<int?> CreateCommande(Commande commande);
        Task<bool> CreateDetailCommande(List<DetailCommande> detailCommandes);
        Task<List<Client>> GetClients(string Ice, string Cnie, string RS);
        Task<List<Commande>> GetCommandes(List<int>  ClientId, DateTime? DateCommande, string DateDebutSearch, string DateFinSearch);
        Task<List<DetailCommande>> GetListDetailsCommande(int? id);
        Task<List<string>> GetCommandesStatuts(int? id);
        Task<Commande> GetCommande(int? Id);
        Task<List<TarifPompeRef>> GetTarifPompeRefs();
        Task<double> GetTarifPompe(int Id);
        Task<List<Commande>> GetCommandesPT(List<int>  ClientId, DateTime? DateCommande, string DateDebutSearch, string DateFinSearch);
        Task<Commande> GetCommandeOnly(int? Id);
        Task<DetailCommande> GetDetailCommande(int? Id);
        Task CreateValidation(Validation validation);
        Task<Dictionary<int?, double?>> GetTarifsByArticleIds(List<int?> Ids);
        Task<List<Commande>> GetCommandesDAPBE(List<int>  ClientId, DateTime? DateCommande, string dateDebutSearch, string dateFinSearch);
        Task<List<Commande>> GetCommandesRC(List<int>  ClientId, DateTime? DateCommande, string dateDebutSearch, string dateFinSearch);
        Task<List<Commande>> GetCommandesCV(List<int>  ClientId, DateTime? DateCommande, string dateDebutSearch, string dateFinSearch);
        Task<List<Commande>> GetCommandesRL(List<int> ClientIds, DateTime? DateCommande, string DateDebutSearch, string DateFinSearch);
        Task<List<Ville>> GetVilles();
        Task<List<Pays>> GetPays();
        Task<bool> UpdateChantier(int id, Chantier chantier);
        Task<bool> UpdateClient(int id, Client client);
        Task<bool> UpdateDetailCommande(List<DetailCommande> detailCommandes);
        Client FindFormulaireClient(string Ice, string Cnie, string Rs);
        Task<List<Validation>> GetListValidation(int commandeId);
        Task<bool> SetCommande(int commandeId);
        Task<List<Commande>> GetCommandesValide(List<int> clientId, DateTime? dateTime, string dateDebutSearch,
            string dateFinSearch);

    }
}
