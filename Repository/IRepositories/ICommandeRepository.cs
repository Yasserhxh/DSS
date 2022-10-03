using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface ICommandeRepository
    {
        IEnumerable<FormeJuridique> GetFormeJuridiques();
        IEnumerable<TypeChantier> GetTypeChantiers();
        IEnumerable<Zone> GetZones();
        IEnumerable<Article> GetArticles();
        IEnumerable<DelaiPaiement> GetDelaiPaiements();
        IEnumerable<CentraleBeton> GetCentraleBetons();
        Task<double> GetTarifArticle(int Id);
        Task<double> GetTarifZone(int Id);
        Task<int?> CreateChantier(Chantier chantier);
        Task<int?> CreateClient(Client client);
        Task<int?> CreateCommande(Commande commande);
        Task<bool> CreateDetailCommande(List<DetailCommande> detailCommandes);
        Task<IEnumerable<Client>> GetClients(string Ice, string Cnie, string RS);
        Task<IEnumerable<Commande>> GetCommandes(int? ClientId, DateTime? DateCommande);
        Task<Commande> GetCommande(int? Id);
        IEnumerable<TarifPompeRef> GetTarifPompeRefs();
        Task<double> GetTarifPompe(int Id);
        Task<IEnumerable<Commande>> GetCommandesPT(int? ClientId, DateTime? DateCommande);
        Task<Commande> GetCommandeOnly(int? Id);
        Task<DetailCommande> GetDetailCommande(int? Id);
        Task CreateValidation(Validation validation);
        Task<Dictionary<int?, double?>> GetTarifsByArticleIds(List<int?> Ids);
        Task<IEnumerable<Commande>> GetCommandesDAPBE(int? ClientId, DateTime? DateCommande);
        Task<IEnumerable<Commande>> GetCommandesRC(int? ClientId, DateTime? DateCommande);
        Task<IEnumerable<Commande>> GetCommandesCV(int? ClientId, DateTime? DateCommande);
        Task<IEnumerable<Commande>> GetCommandesRL(int? ClientId, DateTime? DateCommande);
        Task<bool> UpdateChantier(int id, Chantier chantier);
        Task<bool> UpdateClient(int id, Client client);
        Task<bool> UpdateDetailCommande(List<DetailCommande> detailCommandes);
    }
}
