﻿using Domain.Entities;

namespace Repository.IRepositories
{
    public interface ICommandeRepository
    {
        Task<List<FormeJuridique>> GetFormeJuridiques();
        Task<List<TypeChantier>> GetTypeChantiers();
        Task<List<Zone>> GetZones();
        Task<List<Article>> GetArticles(int? villeId);
        Task<List<DelaiPaiement>> GetDelaiPaiements();
        Task<List<CentraleBeton>> GetCentraleBetons();
        Task<double> GetTarifArticle(int Id);
        Task<double> GetTarifZone(int Id);
        Task<int?> CreateChantier(Chantier chantier);
        Task<int?> CreateClient(Client client);
        Task<int?> CreateCommande(Commande commande);
        Task<int?>  CreateCommandeV(CommandeV commande);
        Task<bool> CreateDetailCommande(List<DetailCommande> detailCommandes);
        Task<bool> CreateDetailCommandeV(List<DetailCommandeV> detailCommandesV);
        Task<Article> GetArticleByDesi(string articleDesi);
        Task<List<Client>> GetClients(string Ice, string Cnie, string RS);
        Task<List<Commande>> GetCommandes(List<int>  ClientId, DateTime? DateCommande, string DateDebutSearch, string DateFinSearch, string Email);
        Task<List<DetailCommande>> GetListDetailsCommande(int? id); 
        Task<List<DetailCommandeV>> GetListDetailsCommandeV(int? id);
        Task<List<ValidationEtat>> GetCommandesStatuts(int? id);
        Task<List<Prospect>> GetListProspects();
        Task<Commande> GetCommande(int? Id);
        Task<int?> CreateProspect(Prospect prospect);
        Task<bool> CreateDetailOffreDePrix(List<OffreDePrix_Details> detailsOffre);
        Task<int?> CreateOffreDePrix(OffreDePrix offre);
        Task<CommandeV> GetCommandeV(int? Id);
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
        Task<bool> AddStatutCommande(CommandeStatut commandeStatut);
        Client FindFormulaireClient(string Ice, string Cnie, string Rs, int? clientId);
        Chantier FindFormulaireChantier(int? chantierId);
        Task<List<Validation>> GetListValidation(int commandeId);
        Task<bool> SetCommande(int commandeId);
        Task<List<CommandeV>> GetCommandesValide(List<int> clientId, DateTime? dateTime, string dateDebutSearch,
            string dateFinSearch);

        Task<List<CommandeV>> GetCommandesV(List<int> clientId, DateTime? dateCommande, string dateDebutSearch,
            string dateFinSearch);
    }
}
