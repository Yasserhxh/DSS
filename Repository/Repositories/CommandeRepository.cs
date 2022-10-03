using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.IRepositories;
using Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class CommandeRepository : ICommandeRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        public CommandeRepository(ApplicationDbContext db, IUnitOfWork unitOfWork)
        {
            _db = db;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<FormeJuridique> GetFormeJuridiques()
        {
            return _db.FormeJuridiques.AsEnumerable();
        }
        public IEnumerable<TypeChantier> GetTypeChantiers()
        {
            return _db.TypeChantiers.AsEnumerable();
        }
        public IEnumerable<Zone> GetZones()
        {
            return _db.Zones.AsEnumerable();
        }
        public IEnumerable<Article> GetArticles()
        {
            return _db.Articles.AsEnumerable();
        }
        public IEnumerable<DelaiPaiement> GetDelaiPaiements()
        {
            return _db.DélaiPaiements.AsEnumerable();
        }
        public IEnumerable<CentraleBeton> GetCentraleBetons()
        {
            return _db.CentraleBetons.AsEnumerable();
        }

        public async Task<double> GetTarifArticle(int Id)
        {
            var Article = await _db.Articles.FirstOrDefaultAsync(x => x.Article_Id == Id);
            if (Article.Tarif != null)
                return (double)Article.Tarif;
            return 0;         
        }

        public async Task<int?> CreateChantier(Chantier chantier)
        {
            await _db.Chantiers.AddAsync(chantier);
            var confirm = await _unitOfWork.Complete();
            if (confirm > 0)
                return chantier.Ctn_Id;
            else
                return null;
        }

        public async Task<int?> CreateClient(Client client)
        {
            await _db.Clients.AddAsync(client);
            var confirm = await _unitOfWork.Complete();
            if (confirm > 0)
                return client.Client_Id;
            else
                return null;
        }

        public async Task<int?> CreateCommande(Commande commande)
        {
            await _db.Commandes.AddAsync(commande);
            var confirm = await _unitOfWork.Complete();
            if (confirm > 0)
                return commande.IdCommande;
            else
                return null;
        }
        public async Task<bool> CreateDetailCommande(List<DetailCommande> detailCommandes)
        {
            await _db.DetailCommandes.AddRangeAsync(detailCommandes);
            var confirm = await _unitOfWork.Complete();
            if (confirm > 0)
                return true;
            else
                return false;
        }

        public async Task<IEnumerable<Client>> GetClients(string Ice, string Cnie, string RS)
        {
            var query = _db.Clients.AsQueryable();
            if (Ice != null)
            {
                query = query.Where(d => d.Ice == Ice);
            }
            if (Cnie != null)
            {
                query = query.Where(d => d.Cnie == Cnie);
            }
            if (RS != null)
            {
                query = query.Where(d => d.RaisonSociale == RS);
            }
            return await query
                .Include(d => d.Chantier)
                .Include(d => d.Ville)
                .Include(d => d.Pays)
                .ToListAsync();
        }

        public async Task<IEnumerable<Commande>> GetCommandes(int? ClientId, DateTime? DateCommande)
        {
            var query = _db.Commandes.AsQueryable();
            if (ClientId.HasValue)
            {
                query = query.Where(d => d.IdClient == ClientId);
            }
            if (DateCommande.HasValue)
            {
                query = query.Where(d => d.DateCommande.Value.Date == DateCommande);
            }
            return await query
                .Include(d => d.Chantier)
                .Include(d => d.Client)
                .Include(d => d.Statut)
                .ToListAsync();
        }

        public async Task<IEnumerable<Commande>> GetCommandesPT(int? ClientId, DateTime? DateCommande)
        {
            var query = _db.Commandes.Where(x => x.IdStatut == Statuts.EtudeEtPropositionDePrix)
                .AsQueryable();
            if (ClientId.HasValue)
            {
                query = query.Where(d => d.IdClient == ClientId);
            }
            if (DateCommande.HasValue)
            {
                query = query.Where(d => d.DateCommande.Value.Date == DateCommande);
            }
            return await query
                .Include(d => d.Chantier)
                .Include(d => d.Client)
                .Include(d => d.Statut)
                .ToListAsync();
        }

        public async Task<Commande> GetCommande(int? Id)
        {
            var cmd = await _db.Commandes
                .Include(x => x.DetailCommandes)
                .ThenInclude(a => a.Article)
                .Include(x => x.DetailCommandes)
                .Include(d => d.Chantier.ZONE_CHANTIER)
                .Include(d => d.Client)
                .FirstOrDefaultAsync(x => x.IdCommande == Id);
            return cmd;
        }
        public async Task<Commande> GetCommandeOnly(int? Id)
        {
            var cmd = await _db.Commandes
                .Include(x => x.DetailCommandes)
                .Include(d => d.Chantier.ZONE_CHANTIER)
                .FirstOrDefaultAsync(x => x.IdCommande == Id);
            return cmd;
        }
        public async Task<DetailCommande> GetDetailCommande(int? Id)
        {
            var cmd = await _db.DetailCommandes
                .Include(x => x.Commande)
                    .ThenInclude(x => x.Chantier)
                        .ThenInclude(x => x.ZONE_CHANTIER)
                .FirstOrDefaultAsync(x => x.IdDetailCommande == Id);
            return cmd;
        }
        public async Task<double> GetTarifZone(int Id)
        {
            var Zone = await _db.Zones.FirstOrDefaultAsync(x => x.Zone_Id == Id);
            if (Zone != null)
                return (double)Zone.Zone_Prix;
            return 0;
        }

        public IEnumerable<TarifPompeRef> GetTarifPompeRefs()
        {
            return _db.TarifPompeRefs.ToList();
        }

        public async Task<double> GetTarifPompe(int Id)
        {
            var tarif = await _db.TarifPompeRefs.FirstOrDefaultAsync(x => x.Tpr_Id == Id);
            if (tarif != null)
                return (double)tarif.LongFleche_Prix;
            return 0;
        }
        public async Task CreateValidation(Validation validation)
        {
            await _db.Validations.AddAsync(validation);
        }

        public async Task<Dictionary<int?, double?>> GetTarifsByArticleIds(List<int?> Ids)
        {
            var A = await _db.Articles.Where(x => Ids.Contains(x.Article_Id)).ToDictionaryAsync(x => (int?)x.Article_Id, x=> x.Tarif);
            return A;
        }

        public async Task<IEnumerable<Commande>> GetCommandesDAPBE(int? ClientId, DateTime? DateCommande)
        {
            var query = _db.Commandes
                .Include(d => d.Chantier)
                .Include(d => d.Client)
                .Include(d => d.Statut)
                .Include(x => x.DetailCommandes)
                .ThenInclude(x => x.Article)
                .Where(x => x.IdStatut == Statuts.ValidationDeLoffreDePrix)
                .AsQueryable();

            if (ClientId.HasValue)
            {
                query = query.Where(d => d.IdClient == ClientId);
            }
            if (DateCommande.HasValue)
            {
                query = query.Where(d => d.DateCommande.Value.Date == DateCommande);
            }
            var commandes = await query.ToListAsync();

            List<Commande> result = new List<Commande>();
            foreach (var cmd in commandes)
            {
                var tarifs = await GetTarifsByArticleIds(cmd.DetailCommandes.Select(x => x.IdArticle).ToList());
                if (cmd.DetailCommandes.Any(x => tarifs[x.IdArticle] - (double)x.Montant >= 10 || x.IdArticle == 4))
                {
                    result.Add(cmd);
                    continue;
                }
            }
            return result;
        }

        public async Task<IEnumerable<Commande>> GetCommandesRC(int? ClientId, DateTime? DateCommande)
        {
            var query = _db.Commandes
                .Include(d => d.Chantier)
                .Include(d => d.Client)
                .Include(d => d.Statut)
                .Include(x => x.DetailCommandes)
                .ThenInclude(x => x.Article)
                .Where(x => x.IdStatut == Statuts.ValidationDeLoffreDePrix)
                .AsQueryable();

            if (ClientId.HasValue)
            {
                query = query.Where(d => d.IdClient == ClientId);
            }
            if (DateCommande.HasValue)
            {
                query = query.Where(d => d.DateCommande.Value.Date == DateCommande);
            }
            var commandes = await query.ToListAsync();

            List<Commande> result = new List<Commande>();
            foreach (var cmd in commandes)
            {
                var tarifs = await GetTarifsByArticleIds(cmd.DetailCommandes.Select(x => x.IdArticle).ToList());
                if (cmd.DetailCommandes.Any(x => 5 < tarifs[x.IdArticle] - (double)x.Montant && tarifs[x.IdArticle] - (double)x.Montant < 10))
                {
                    result.Add(cmd);
                    continue;
                }
            }
            return result;
        }
        public async Task<IEnumerable<Commande>> GetCommandesCV(int? ClientId, DateTime? DateCommande)
        {
            var query = _db.Commandes
                .Include(d => d.Chantier)
                .Include(d => d.Client)
                .Include(d => d.Statut)
                .Include(x => x.DetailCommandes)
                .ThenInclude(x => x.Article)
                .Where(x => x.IdStatut == Statuts.ValidationDeLoffreDePrix)
                .AsQueryable();

            if (ClientId.HasValue)
            {
                query = query.Where(d => d.IdClient == ClientId);
            }
            if (DateCommande.HasValue)
            {
                query = query.Where(d => d.DateCommande.Value.Date == DateCommande);
            }
            var commandes = await query.ToListAsync();

            List<Commande> result = new List<Commande>();
            foreach (var cmd in commandes)
            {
                var tarifs = await GetTarifsByArticleIds(cmd.DetailCommandes.Select(x => x.IdArticle).ToList());
                if (cmd.DetailCommandes.Any(x => tarifs[x.IdArticle] - (double)x.Montant <= 5))
                {
                    result.Add(cmd);
                    continue;
                }
            }
            return result;
        }

        public async Task<IEnumerable<Commande>> GetCommandesRL(int? ClientId, DateTime? DateCommande)
        {
            var query = _db.Commandes
                .Where(x => x.IdStatut == Statuts.FixationDePrixDuTransport)
                .AsQueryable();
            if (ClientId.HasValue)
            {
                query = query.Where(d => d.IdClient == ClientId);
            }
            if (DateCommande.HasValue)
            {
                query = query.Where(d => d.DateCommande.Value.Date == DateCommande);
            }
            return await query
                .Include(d => d.Chantier.ZONE_CHANTIER)
                .Include(d => d.Client)
                .Include(d => d.Statut)
                .Include(d => d.Tarif_Pompe)
                .ToListAsync();
        }

        public async Task<bool> UpdateChantier(int id, Chantier chantier)
        {
            try
            {
                var Chantier = await _db.Chantiers.FirstOrDefaultAsync(x => x.Ctn_Id == id);

                Chantier.Ctn_Zone_Id = chantier.Ctn_Zone_Id;
                Chantier.Ctn_Adresse = chantier.Ctn_Adresse;
                Chantier.Ctn_Ctr_Id = chantier.Ctn_Ctr_Id;
                Chantier.Ctn_Nom = chantier.Ctn_Nom;
                Chantier.MaitreOuvrage = chantier.MaitreOuvrage;
                Chantier.Rayon = chantier.Rayon;
                Chantier.VolumePrevisonnel = chantier.VolumePrevisonnel;
                Chantier.Ctn_Tc_Id = chantier.Ctn_Tc_Id;

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }          
        }

        public async Task<bool> UpdateClient(int id, Client client)
        {
            try
            {
                var Client = await _db.Clients.FirstOrDefaultAsync(x => x.Client_Id == id);

                Client.RaisonSociale = client.RaisonSociale;
                Client.Ice = client.Ice;
                Client.Adresse = client.Adresse;
                Client.Cnie = client.Cnie;
                Client.Destinataire_Interlocuteur = client.Destinataire_Interlocuteur;
                Client.Gsm = client.Gsm;
                Client.FormeJuridique_Id = client.FormeJuridique_Id;
                client.Email = client.Email;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateDetailCommande(List<DetailCommande> detailCommandes)
        {
            try
            {
                foreach (var det in detailCommandes)
                {
                    var detail = await _db.DetailCommandes.FirstOrDefaultAsync(x => x.IdDetailCommande == det.IdDetailCommande);
                    detail.Montant = det.Montant;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
