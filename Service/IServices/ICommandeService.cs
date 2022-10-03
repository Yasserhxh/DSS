using Domain.Models;
using Domain.Models.Commande;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices
{
    public interface ICommandeService
    {
        IEnumerable<FormeJuridiqueModel> GetFormeJuridiques();
        IEnumerable<TypeChantierModel> GetTypeChantiers();
        IEnumerable<ZoneModel> GetZones();
        IEnumerable<ArticleModel> GetArticles();
        IEnumerable<DelaiPaiementModel> GetDelaiPaiements();
        IEnumerable<CentraleBetonModel> GetCentraleBetons();
        Task<double> GetTarifArticle(int Id);
        Task<bool> CreateCommande(CommandeViewModel commandeViewModel);
        Task<IEnumerable<ClientModel>> GetClients(string Ice = null, string Cnie = null, string RS = null);
        Task<IEnumerable<CommandeModel>> GetCommandes(int? ClientId, DateTime? DateCommande);
        Task<CommandeModel> GetCommande(int? id);
        IEnumerable<TarifPompeRefModel> GetTarifPompeRefs();
        Task<double> GetTarifZone(int Id);
        Task<double> GetTarifPompe(int Id);
        Task<IEnumerable<CommandeModel>> GetCommandesPT(int? ClientId, DateTime? DateCommande);
        Task<bool> ProposerPrix(int Id, decimal Tarif, string UserName);
        Task<IEnumerable<CommandeModel>> GetCommandesDAPBE(int? ClientId, DateTime? DateCommande);
        Task<IEnumerable<CommandeModel>> GetCommandesRC(int? ClientId, DateTime? DateCommande);
        Task<IEnumerable<CommandeModel>> GetCommandesCV(int? ClientId, DateTime? DateCommande);
        Task<bool> ProposerPrixDABPE(int Id, decimal Tarif);
        Task<bool> RefuserCommande(int Id, string Commentaire, string UserName);
        Task<bool> ValiderCommande(int Id, string Commentaire, string UserName);
        Task<IEnumerable<CommandeModel>> GetCommandesRL(int? ClientId, DateTime? DateCommande);
        Task<bool> UpdateCommande(int id, CommandeViewModel commandeViewModel, string UserName);
        Task<bool> FixationPrixTransport(int Id, double VenteT, double VenteP);
    }
}
