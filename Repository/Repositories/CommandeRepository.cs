﻿using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.IRepositories;
using Repository.UnitOfWork;

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

        public async Task<List<FormeJuridique>> GetFormeJuridiques()
        {
            return await _db.FormeJuridiques.ToListAsync();
        }
        public async Task<List<TypeChantier>> GetTypeChantiers()
        {
            return await _db.TypeChantiers.ToListAsync();
        }
        public async Task<List<Zone>> GetZones()
        {
            return await _db.Zones.ToListAsync();
        }
        public async Task<List<Article>> GetArticles(int? villeId)
        {
            var query =  _db.Articles.Where(p => p.Article_Id != 14 && p.Article_Id != 15).AsQueryable();
            if(villeId is not null)
                query = query.Where(p=>p.RegionId ==  villeId);

            return await query.ToListAsync();
        }
        
        public async Task<List<DelaiPaiement>> GetDelaiPaiements()
        {
            return await _db.DélaiPaiements.ToListAsync();
        }
        public async Task<List<CentraleBeton>> GetCentraleBetons()
        {
            return await _db.CentraleBetons.ToListAsync();
        }

        public async Task<double> GetTarifArticle(int Id)
        {
            var article = await _db.Articles.FirstOrDefaultAsync(x => x.Article_Id == Id);
            if (article is { Tarif: { } })
                return (double)article.Tarif;
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
            return null;
        }
        public async Task<int?> CreateProspect(Prospect prospect)
        {
            await _db.Prospects.AddAsync(prospect);
            var confirm = await _unitOfWork.Complete();
            if (confirm > 0)
                return prospect.IdProspect;
            return null;
        } 
        public async Task<int?> CreateOffreDePrix(OffreDePrix offre)
        {
            await _db.OffreDePrix.AddAsync(offre);
            var confirm = await _unitOfWork.Complete();
            if (confirm > 0)
                return offre.OffreId;
            return null;
        }

        public async Task<int?> CreateCommande(Commande commande)
        {
            await _db.Commandes.AddAsync(commande);
            var confirm = await _unitOfWork.Complete();
            if (confirm > 0)
                return commande.IdCommande;
            return null;
        } 
        public async Task<int?> CreateCommandeV(CommandeV commande)
        {
            await _db.CommandesV.AddAsync(commande);
            var confirm = await _unitOfWork.Complete();
            if (confirm > 0)
                return commande.IdCommande;
            return null;
        }
        public async Task<bool> CreateDetailCommande(List<DetailCommande> detailCommandes)
        {
            await _db.DetailCommandes.AddRangeAsync(detailCommandes);
            var confirm = await _unitOfWork.Complete();
            return confirm > 0;
        }     
        public async Task<bool> AddStatutCommande(CommandeStatut commandeStatut)
        {
            await _db.CommandeStatuts.AddAsync(commandeStatut);
            var confirm = await _unitOfWork.Complete();
            return confirm > 0;
        }    
        public async Task<bool> CreateDetailOffreDePrix(List<OffreDePrix_Details> detailsOffre)
        {
            await _db.OffreDePrixDetails.AddRangeAsync(detailsOffre);
            var confirm = await _unitOfWork.Complete();
            return confirm > 0;
        }   
        public async Task<bool> CreateDetailCommandeV(List<DetailCommandeV> detailCommandesV)
        {
            await _db.DetailCommandesV.AddRangeAsync(detailCommandesV);
            var confirm = await _unitOfWork.Complete();
            return confirm > 0;
        }
        public async Task<Article> GetArticleByDesi(string articleDesi)
        {
            return await _db.Articles.Where(p=>p.Designation == articleDesi).FirstOrDefaultAsync();
            
        }

        public async Task<List<Client>> GetClients(string ice, string cnie, string rs)
        {
            var query = _db.Clients.AsQueryable();
            if (!string.IsNullOrEmpty(ice))
                query = query.Where(d => d.Ice == ice);
            if (!string.IsNullOrEmpty(cnie))
                query = query.Where(d => d.Cnie == cnie);
            if (!string.IsNullOrEmpty(rs))  
                query = query.Where(d => d.RaisonSociale == rs);
            
            return await query
                .Include(d => d.Chantier)
                .Include(d => d.Ville)
                .Include(d => d.Pays)
                .ToListAsync();
        }

        public async Task<List<Commande>> GetCommandes(List<int> clientId, DateTime? dateCommande, string dateDebutSearch, string dateFinSearch, string Email )
        {
            var query = _db.Commandes.Where(d => clientId.Contains((int)d.IdClient) && d.IsProspection == true && d.CommercialEmail == Email).AsQueryable();
            if (dateCommande is not null)
            {
                query = query.Where(d => d.DateCommande.Value.Date == dateCommande);
            }
            if (!string.IsNullOrEmpty(dateDebutSearch))query = query.Where(x =>
                x.DateCommande.Value.Date >= DateTime.ParseExact(dateDebutSearch, "dd/MM/yyyy", null).Date );
            
            if(!string.IsNullOrEmpty(dateFinSearch))
                query = query.Where(x =>
                    x.DateCommande.Value.Date <= DateTime.ParseExact(dateFinSearch, "dd/MM/yyyy", null).Date );

            return await query
                .Include(d => d.Chantier).ThenInclude(p=>p.Type_Chantier)
                .Include(d => d.Chantier).ThenInclude(p=>p.ZONE_CHANTIER)
                .Include(d => d.Chantier).ThenInclude(p=>p.Centrale_Beton)
                .Include(d => d.Client).ThenInclude(p=>p.Forme_Juridique)
                .Include(d => d.Client).ThenInclude(p=>p.Ville)
                .Include(d => d.Client).ThenInclude(p=>p.Pays)
                .Include(d => d.Statut)
                .Include(p=>p.Tarif_Pompe)
                .Include(d => d.DetailCommandes)
                .ThenInclude(p=>p.Article)
                .Include(d => d.DetailCommandes)
                .ThenInclude(p=>p.Unite)
                .ToListAsync();
            //return await query.ToListAsync();
        }
        public async Task<List<CommandeV>> GetCommandesV(List<int> clientId, DateTime? dateCommande, string dateDebutSearch, string dateFinSearch)
        {
            var query = _db.CommandesV.Where(d => clientId.Contains((int)d.IdClient)).AsQueryable();
            if (dateCommande is not null)
            {
                query = query.Where(d => d.DateCommande.Value.Date == dateCommande);
            }
            if (!string.IsNullOrEmpty(dateDebutSearch))query = query.Where(x =>
                x.DateCommande.Value.Date >= DateTime.ParseExact(dateDebutSearch, "dd/MM/yyyy", null).Date );
            
            if(!string.IsNullOrEmpty(dateFinSearch))
                query = query.Where(x =>
                    x.DateCommande.Value.Date <= DateTime.ParseExact(dateFinSearch, "dd/MM/yyyy", null).Date );

            return await query
                .Include(d => d.Chantier).ThenInclude(p=>p.Type_Chantier)
                .Include(d => d.Chantier).ThenInclude(p=>p.ZONE_CHANTIER)
                .Include(d => d.Chantier).ThenInclude(p=>p.Centrale_Beton)
                .Include(d => d.Client).ThenInclude(p=>p.Forme_Juridique)
                .Include(d => d.Client).ThenInclude(p=>p.Ville)
                .Include(d => d.Client).ThenInclude(p=>p.Pays)
                .Include(d => d.Statut)
                .Include(p=>p.Tarif_Pompe)
                .Include(d => d.DetailCommandes)
                .ThenInclude(p=>p.Article)
                .Include(d => d.DetailCommandes)
                .ThenInclude(p=>p.Unite)
                .ToListAsync();
            //return await query.ToListAsync();
        }

        public async Task<List<Commande>> GetCommandesPT(List<int> clientId, DateTime? dateCommande, string dateDebutSearch, string dateFinSearch)
        {
            // x.IdStatut == Statuts.EnCoursDeTraitement && 
            
            var query = _db.Commandes
                .Where(x => x.CommandeStatuts.Any(p => p.StatutId == Statuts.EtudeEtPropositionDePrix) == true
                            && x.IsProspection == true  /*&& x.IdStatut == Statuts.EnCoursDeTraitement*/)
                .AsQueryable();
            if (clientId.Any())
                query = query.Where(d => clientId.Contains((int)d.IdClient));
            
            if (dateCommande.HasValue)
                query = query.Where(d => d.DateCommande.Value.Date == dateCommande);
            
            if (!string.IsNullOrEmpty(dateDebutSearch))query = query.Where(x =>
                x.DateCommande.Value.Date >= DateTime.ParseExact(dateDebutSearch, "dd/MM/yyyy", null).Date);
            
            if(!string.IsNullOrEmpty(dateFinSearch))
                query = query.Where(x =>
                    x.DateCommande.Value.Date <= DateTime.ParseExact(dateFinSearch, "dd/MM/yyyy", null).Date);
           // var validateursQuery = _db.Validations.Where(p=>query.Any(x=>x.IdCommande==p.IdCommande))
            return await query
                .Include(d => d.Chantier).ThenInclude(p=>p.Type_Chantier)
                .Include(d => d.Chantier).ThenInclude(p=>p.ZONE_CHANTIER)
                .Include(d => d.Chantier).ThenInclude(p=>p.Centrale_Beton)
                .Include(d => d.Client).ThenInclude(p=>p.Forme_Juridique)
                .Include(d => d.Client).ThenInclude(p=>p.Ville)
                .Include(d => d.Client).ThenInclude(p=>p.Pays)
                .Include(d => d.Statut)
                .Include(p=>p.Tarif_Pompe)
                .Include(d => d.DetailCommandes)
                .ThenInclude(p=>p.Article)
                .Include(d => d.DetailCommandes)
                .ThenInclude(p=>p.Unite)
                .ToListAsync();
        }

        public async Task<List<ValidationEtat>> GetCommandesStatuts(int? id)
        {
            var listeStatus = await _db.CommandeStatuts.Where(p => p.CommandeId == id)
                .Select(p => p.Statut).ToListAsync();
            var ListeStatutsRes = new List<string>();
            var ValidationEtatList = new List<ValidationEtat>();
            foreach (var item in listeStatus)
            {
                switch (item.IdStatut)
                {
                    case Statuts.EtudeEtPropositionDePrix:
                    {
                        var validateur =  _db.Validations.FirstOrDefault(p => p.IdCommande == id && p.Fonction == "Prescripteur technique");
                        if (validateur == null)
                        {
                            var statut = "En attente: Prescripteur technique";
                            
                            ListeStatutsRes.Add(statut);
                            ValidationEtatList.Add(new ValidationEtat
                            {
                                Etat = "En attente: Prescripteur technique"
                            });
                        }
                        else
                        {
                            var statut = "Validée par: Prescripteur technique";
                            ListeStatutsRes.Add(statut);
                            ValidationEtatList.Add(new ValidationEtat
                            {
                                Etat ="Validée par: Prescripteur technique",
                                NomPrenomVal = validateur.Nom + validateur.Prenom,
                                DateValdiation = validateur.Date
                            });
                        }
                        break;
                    }
                    case Statuts.FixationDePrixDuTransport:
                    {
                        var validateur =  _db.Validations.FirstOrDefault(p => p.IdCommande == id && p.Fonction == "Responsable logistique");
                        if (validateur == null)
                        { 
                            var statut = "En attente: Responsable logistique";
                            ListeStatutsRes.Add(statut);
                            ValidationEtatList.Add(new ValidationEtat
                            {
                                Etat = "En attente: Responsable logistique"
                            });
                        }
                        else
                        {
                            var statut = "Validée par: Responsable logistique";
                            ListeStatutsRes.Add(statut);
                            ValidationEtatList.Add(new ValidationEtat
                            {
                                Etat ="Validée par: Responsable logistique",
                                NomPrenomVal = validateur.Nom + validateur.Prenom,
                                DateValdiation = validateur.Date
                            });
                        }
                        break;
                    }
                    case Statuts.ValidationDeLoffreDePrixDABPE:
                    {
                        var validateur =  _db.Validations.FirstOrDefault(p => p.IdCommande == id && p.Fonction == "DA BPE");
                        if (validateur == null)
                        {
                            var statut = "En attente: Directeur d'activité";
                            ListeStatutsRes.Add(statut);
                            ValidationEtatList.Add(new ValidationEtat
                            {
                                Etat =  "En attente: Directeur d'activité"
                            });
                        }
                        else
                        {
                            var statut = "Validée par: Directeur d'activité";
                            ListeStatutsRes.Add(statut);
                            ValidationEtatList.Add(new ValidationEtat
                            {
                                Etat ="Validée par: Directeur d'activité",
                                NomPrenomVal = validateur.Nom + validateur.Prenom,
                                DateValdiation = validateur.Date
                            });
                        }
                        break;
                    }
                    case Statuts.ValidationDeLoffreDePrixRC:
                    {
                        var validateur =  _db.Validations.FirstOrDefault(p => p.IdCommande == id && p.Fonction == "Responsable commercial");
                        if (validateur == null)
                        {
                            var statut = "En attente: Responsable commercial";
                            ListeStatutsRes.Add(statut);
                            ValidationEtatList.Add(new ValidationEtat
                            {
                                Etat =  "En attente: Responsable commercial"
                            });
                        }
                        else
                        {
                            var statut = "Validée par: Responsable commercial";
                            ListeStatutsRes.Add(statut);
                            ValidationEtatList.Add(new ValidationEtat
                            {
                                Etat ="Validée par: Responsable commercial",
                                NomPrenomVal = validateur.Nom + validateur.Prenom,
                                DateValdiation = validateur.Date
                            });
                        }
                        break;
                    }
                    case Statuts.ValidationDeLoffreDePrixCV:
                    {
                        var validateur =  _db.Validations.FirstOrDefault(p => p.IdCommande == id && p.Fonction == "Chef de ventes");
                        if (validateur == null)
                        {
                            var statut = "En attente: Chef de ventes";
                            ListeStatutsRes.Add(statut);
                            ValidationEtatList.Add(new ValidationEtat
                            {
                                Etat = "En attente: Chef de ventes"
                            });
                        }
                        else
                        {
                            var statut = "Validée par: Chef de ventes";
                            ListeStatutsRes.Add(statut);
                            ValidationEtatList.Add(new ValidationEtat
                            {
                                Etat = "Validée par: Chef de ventes",
                                NomPrenomVal = validateur.Nom + validateur.Prenom,
                                DateValdiation = validateur.Date
                            });
                        }
                        break;
                    }
                        case Statuts.Validé:
                        {
                            ValidationEtatList.Add(new ValidationEtat
                            {
                                Etat = "Validée"
                            });
                        }
                        break;
                }
            }
            return ValidationEtatList;
        }


        public async Task<Commande> GetCommande(int? id)
        {
           /* var cmd = await _db.Commandes
                .Include(x => x.DetailCommandes)
                .ThenInclude(a => a.Article)
                .Include(x => x.DetailCommandes)
                .Include(d => d.Chantier.ZONE_CHANTIER)
                .Include(d => d.Client)
                .FirstOrDefaultAsync(x => x.IdCommande == Id);*/
            var cmd = await _db.Commandes.Where(x => x.IdCommande == id)
                .Include(x => x.DetailCommandes)
                .ThenInclude(a => a.Article)
                .Include(x => x.DetailCommandes)
                .Include(p=>p.CommandeStatuts)
                .Include(d => d.Chantier.ZONE_CHANTIER)
                .Include(d=>d.Chantier.Type_Chantier)
                .Include(d => d.Client.Forme_Juridique)
                .FirstOrDefaultAsync();
            return cmd;
        }
        public async Task<CommandeV> GetCommandeV(int? id)
        {
           /* var cmd = await _db.Commandes
                .Include(x => x.DetailCommandes)
                .ThenInclude(a => a.Article)
                .Include(x => x.DetailCommandes)
                .Include(d => d.Chantier.ZONE_CHANTIER)
                .Include(d => d.Client)
                .FirstOrDefaultAsync(x => x.IdCommande == Id);*/
            var cmd = await _db.CommandesV.Where(x => x.IdCommande == id)
                .Include(x => x.DetailCommandes)
                .ThenInclude(a => a.Article)
                .Include(x => x.DetailCommandes)
                //.Include(p=>p.CommandeStatuts)
                .Include(d => d.Chantier.ZONE_CHANTIER)
                .Include(d => d.Client.Forme_Juridique).FirstOrDefaultAsync();
            return cmd;
        }
        public async Task<Commande> GetCommandeOnly(int? id)
        {
            var cmd = await _db.Commandes
                .Include(x => x.DetailCommandes)
                .Include(d => d.Chantier.ZONE_CHANTIER)
                .Include(x=>x.CommandeStatuts)
                .FirstOrDefaultAsync(x => x.IdCommande == id);
            return cmd;
        } 
        public async Task<List<DetailCommande>> GetListDetailsCommande(int? id)
        {
            var cmd = await _db.DetailCommandes
                .Where(x => x.IdCommande == id)
                .Include(d => d.Article)
                .Include(d => d.Unite)
                .ToListAsync();
            return cmd;
        }  
        public async Task<List<DetailCommandeV>> GetListDetailsCommandeV(int? id)
        {
            var cmd = await _db.DetailCommandesV
                .Where(x => x.IdCommande == id)
                .Include(d => d.Article)
                .Include(d => d.Unite)
                .ToListAsync();
            return cmd;
        }
        public async Task<DetailCommande> GetDetailCommande(int? id)
        {
            var cmd = await _db.DetailCommandes
                .Include(x => x.Commande)
                    .ThenInclude(x => x.Chantier)
                        .ThenInclude(x => x.ZONE_CHANTIER)
                .FirstOrDefaultAsync(x => x.IdDetailCommande == id);
            return cmd;
        }
        public async Task<double> GetTarifZone(int id)
        {
            var zone = await _db.Zones.FirstOrDefaultAsync(x => x.Zone_Id == id);
            if (zone != null)
                return (double)zone.Zone_Prix;
            return 0;
        }

        public async Task<List<TarifPompeRef>> GetTarifPompeRefs()
        {
            return await _db.TarifPompeRefs.ToListAsync();
        }
        public async Task<List<Prospect>> GetListProspects()
        {
            return await _db.Prospects.Where(p=>p.CheckOffre == false)
                .Include(p=>p.Chantier).ThenInclude(p=>p.Type_Chantier)
                .Include(p=>p.Chantier).ThenInclude(p=>p.ZONE_CHANTIER)
                .Include(p=>p.Chantier).ThenInclude(p=>p.Centrale_Beton)
                .Include(p=>p.Client).ThenInclude(p=>p.Forme_Juridique)
                .Include(p=>p.Client).ThenInclude(p=>p.Ville)
                .Include(p=>p.Client).ThenInclude(p=>p.Pays)
                .ToListAsync();
        }

        public async Task<double> GetTarifPompe(int id)
        {
            var tarif = await _db.TarifPompeRefs.FirstOrDefaultAsync(x => x.Tpr_Id == id);
            if (tarif != null)
                return (double)tarif.LongFleche_Prix;
            return 0;
        }
        public async Task CreateValidation(Validation validation)
        {
            await _db.Validations.AddAsync(validation);
            await _unitOfWork.Complete();
        }

        public async Task<List<Validation>> GetListValidation(int commandeId) =>
             await _db.Validations.Where(p => p.IdCommande == commandeId)
                 .ToListAsync();
        
        public async Task<Dictionary<int?, double?>> GetTarifsByArticleIds(List<int?> ids)
        {
            var A = await _db.Articles.Where(x => ids.Contains(x.Article_Id)).ToDictionaryAsync(x => (int?)x.Article_Id, x=> x.Tarif);
            return A;
        }

        public async Task<List<Commande>> GetCommandesDAPBE(List<int> clientId, DateTime? dateCommande, string dateDebutSearch, string dateFinSearch)
        {
            var query = _db.Commandes
                    .Include(d => d.Chantier).ThenInclude(p=>p.Type_Chantier)
                    .Include(d => d.Chantier).ThenInclude(p=>p.ZONE_CHANTIER)
                    .Include(d => d.Chantier).ThenInclude(p=>p.Centrale_Beton)
                    .Include(d => d.Client).ThenInclude(p=>p.Forme_Juridique)
                    .Include(d => d.Client).ThenInclude(p=>p.Ville)
                    .Include(d => d.Client).ThenInclude(p=>p.Pays)
                    .Include(d => d.Statut)
                    .Include(p=>p.Tarif_Pompe)
                    .Include(d => d.DetailCommandes)
                    .ThenInclude(p=>p.Article)
                    .Include(d => d.DetailCommandes)
                    .ThenInclude(p=>p.Unite)
                .Where(x =>   x.CommandeStatuts.Any(p => p.StatutId == Statuts.ValidationDeLoffreDePrixDABPE) == true    && x.IsProspection == true  /*&& x.IdStatut == Statuts.EnCoursDeTraitement*/)
                .AsQueryable();

            if (clientId.Any())
            {
                query = query.Where(d => clientId.Contains((int)d.IdClient));
            }
            if (dateCommande.HasValue)
            {
                query = query.Where(d => d.DateCommande.Value.Date == dateCommande);
            }
             
            if (!string.IsNullOrEmpty(dateDebutSearch))query = query.Where(x =>
                x.DateCommande.Value.Date >= DateTime.ParseExact(dateDebutSearch, "dd/MM/yyyy", null).Date );
            
            if(!string.IsNullOrEmpty(dateFinSearch))
                query = query.Where(x =>
                    x.DateCommande.Value.Date <= DateTime.ParseExact(dateFinSearch, "dd/MM/yyyy", null).Date );
            var commandes = await query.ToListAsync();

        /*    var result = new List<Commande>();
            foreach (var cmd in commandes)
            {
                var tarifs = await GetTarifsByArticleIds(cmd.DetailCommandes.Select(x => x.IdArticle).ToList());
                if (!cmd.DetailCommandes.Any(x => x.IdArticle != null && x.Montant != null && (tarifs[x.IdArticle] - (double)x.Montant >= 10 || x.IdArticle == 4)))
                    continue;
                result.Add(cmd);
                continue;
            }
            return result;*/
        return commandes;
        }

        public async Task<List<Commande>> GetCommandesRC(List<int>clientId, DateTime? dateCommande, string dateDebutSearch,string dateFinSearch)
        {
            
            var query = _db.Commandes
                    .Include(d => d.Chantier).ThenInclude(p=>p.Type_Chantier)
                    .Include(d => d.Chantier).ThenInclude(p=>p.ZONE_CHANTIER)
                    .Include(d => d.Chantier).ThenInclude(p=>p.Centrale_Beton)
                    .Include(d => d.Client).ThenInclude(p=>p.Forme_Juridique)
                    .Include(d => d.Client).ThenInclude(p=>p.Ville)
                    .Include(d => d.Client).ThenInclude(p=>p.Pays)
                    .Include(d => d.Statut)
                    .Include(p=>p.Tarif_Pompe)
                    .Include(d => d.DetailCommandes)
                    .ThenInclude(p=>p.Article)
                    .Include(d => d.DetailCommandes)
                    .ThenInclude(p=>p.Unite)
                    .Where(x => x.CommandeStatuts.Any(p => p.StatutId == Statuts.ValidationDeLoffreDePrixRC) == true   && x.IsProspection == true /*&& x.IdStatut == Statuts.EnCoursDeTraitement*/)
                    .AsQueryable();

            if (clientId.Any())
            {
                query = query.Where(d => clientId.Contains((int)d.IdClient));
            }
            if (dateCommande.HasValue)
            {
                query = query.Where(d => d.DateCommande.Value.Date == dateCommande);
            }
             
            if (!string.IsNullOrEmpty(dateDebutSearch))
                query = query.Where(x =>
                    x.DateCommande.Value.Date >= DateTime.ParseExact(dateDebutSearch, "dd/MM/yyyy", null).Date );
            
            if(!string.IsNullOrEmpty(dateFinSearch))
                query = query.Where(x =>
                    x.DateCommande.Value.Date <= DateTime.ParseExact(dateFinSearch, "dd/MM/yyyy", null).Date );
            var commandes = await query.ToListAsync();

         /*   var result = new List<Commande>();
            foreach (var cmd in commandes)
            {
                var tarifs = await GetTarifsByArticleIds(cmd.DetailCommandes.Select(x => x.IdArticle).ToList());
                if (!cmd.DetailCommandes.Any(x =>
                        x.Montant != null && x.IdArticle != null && 5 < tarifs[x.IdArticle] - (double)x.Montant &&
                        tarifs[x.IdArticle] - (double)x.Montant < 10)) continue;
                result.Add(cmd);
                continue;
            }*/
            return commandes;
        }

        public async Task<List<Commande>> GetCommandesCV(List<int> clientId, DateTime? dateCommande, string dateDebutSearch, string dateFinSearch)
        {
            var query = _db.Commandes
                    .Include(d => d.Chantier).ThenInclude(p=>p.Type_Chantier)
                    .Include(d => d.Chantier).ThenInclude(p=>p.ZONE_CHANTIER)
                    .Include(d => d.Chantier).ThenInclude(p=>p.Centrale_Beton)
                    .Include(d => d.Client).ThenInclude(p=>p.Forme_Juridique)
                    .Include(d => d.Client).ThenInclude(p=>p.Ville)
                    .Include(d => d.Client).ThenInclude(p=>p.Pays)
                    .Include(d => d.Statut)
                    .Include(p=>p.Tarif_Pompe)
                    .Include(d => d.DetailCommandes)
                    .ThenInclude(p=>p.Article)
                    .Include(d => d.DetailCommandes)
                    .ThenInclude(p=>p.Unite)
                .Where(x => x.IdStatut == Statuts.EnCoursDeTraitement
                && /* x.CommandeStatuts.Any(p => p.StatutId == Statuts.ValidationDeLoffreDePrixCV) == true   && */x.IsProspection == true)
                .AsQueryable();

            if (clientId.Any())
            {
                query = query.Where(d => clientId.Contains((int)d.IdClient));
            }
            if (dateCommande.HasValue)
            {
                query = query.Where(d => d.DateCommande.Value.Date == dateCommande);
            }
             
            if (!string.IsNullOrEmpty(dateDebutSearch))query = query.Where(x =>
                x.DateCommande.Value.Date >= DateTime.ParseExact(dateDebutSearch, "dd/MM/yyyy", null).Date );
            
            if(!string.IsNullOrEmpty(dateFinSearch))
                query = query.Where(x =>
                    x.DateCommande.Value.Date <= DateTime.ParseExact(dateFinSearch, "dd/MM/yyyy", null).Date );
            var commandes = await query.ToListAsync();

          //  var result = new List<Commande>();
            /*foreach (var cmd in commandes)
            {
                var tarifs = await GetTarifsByArticleIds(cmd.DetailCommandes.Select(x => x.IdArticle).ToList());
                if (!cmd.DetailCommandes.Any(x => x.Montant != null && x.IdArticle != null && tarifs[x.IdArticle] - (double)x.Montant <= 5)) continue;
                result.Add(cmd);
                continue;
            }*/
            return commandes;
        }

        public async Task<List<Commande>> GetCommandesRL(List<int> clientIds, DateTime? dateCommande, string dateDebutSearch, string dateFinSearch)
        {
            // x.IdStatut == Statuts.EnCoursDeTraitement &&
            var query = _db.Commandes
                .Where(x =>  x.CommandeStatuts.Any(p => p.StatutId == Statuts.FixationDePrixDuTransport) == true && x.IsProspection == true /*&& x.IdStatut == Statuts.EnCoursDeTraitement*/)
                //.Where(x=>x.CommandeStatuts.Any(p=>p.CommandeStatutId == Statuts.FixationDePrixDuTransport))
                .AsQueryable();
            if (clientIds.Any())
                query = query.Where(d => clientIds.Contains((int)d.IdClient));
            
            if (dateCommande.HasValue)
                query = query.Where(d => d.DateCommande.Value.Date == dateCommande);

            if (!string.IsNullOrEmpty(dateDebutSearch))query = query.Where(x =>
                    x.DateCommande.Value.Date >= DateTime.ParseExact(dateDebutSearch, "dd/MM/yyyy", null).Date );
            
            if(!string.IsNullOrEmpty(dateFinSearch))
                query = query.Where(x =>
                    x.DateCommande.Value.Date <= DateTime.ParseExact(dateFinSearch, "dd/MM/yyyy", null).Date );
            
            //if(string.IsNullOrEmpty(dateFinSearch) && string.IsNullOrEmpty(dateDebutSearch))
                //query = query.Where(x => x.DateCommande.Value.Date == DateTime.Now.Date);
            
            return await query
                .Include(d => d.Chantier).ThenInclude(p=>p.Type_Chantier)
                .Include(d => d.Chantier).ThenInclude(p=>p.ZONE_CHANTIER)
                .Include(d => d.Chantier).ThenInclude(p=>p.Centrale_Beton)
                .Include(d => d.Client).ThenInclude(p=>p.Forme_Juridique)
                .Include(d => d.Client).ThenInclude(p=>p.Ville)
                .Include(d => d.Client).ThenInclude(p=>p.Pays)
                .Include(d => d.Statut)
                .Include(p=>p.Tarif_Pompe)
                .Include(d => d.DetailCommandes)
                .ThenInclude(p=>p.Article)
                .Include(d => d.DetailCommandes)
                .ThenInclude(p=>p.Unite)
                .ToListAsync();
        }

        public async Task<List<Ville>> GetVilles() =>  await _db.Villes.ToListAsync();
        public async Task<List<Pays>> GetPays() =>  await _db.Pays.ToListAsync();


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
                    if (detail != null) detail.Montant = det.Montant;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Client FindFormulaireClient(string Ice, string Cnie, string Rs, int? clientId)
        {
            if (!string.IsNullOrEmpty(Ice))
                return _db.Clients.FirstOrDefault(p => p.Ice == Ice);
            if (!string.IsNullOrEmpty(Rs))
                return _db.Clients.FirstOrDefault(p => p.RaisonSociale == Rs);
            if (clientId is not null)
                return _db.Clients.FirstOrDefault(p => p.Client_Id == clientId);

            return !string.IsNullOrEmpty(Cnie) ? _db.Clients.FirstOrDefault(p => p.Cnie == Cnie) : new Client();
        }
        public Chantier FindFormulaireChantier(int? chantierId)
        {
            return chantierId is not null? _db.Chantiers.FirstOrDefault(p => p.Ctn_Id == chantierId) : new Chantier();
        }

        
        public async Task<bool> SetCommande(int commandeId)
        {
            var commande = await _db.Commandes.FirstOrDefaultAsync(p => p.IdCommande == commandeId);
            commande!.IsProspection = false;
            var comfirm =await _unitOfWork.Complete();
            return comfirm > 0;
        }

        public async Task<List<CommandeV>> GetCommandesValide(List<int> clientId, DateTime? dateTime, string dateDebutSearch, string dateFinSearch)
        {
            var query = _db.CommandesV
                //.Where(x =>  x.IsProspection == false )
                //.Where(x=>x.CommandeStatuts.Any(p=>p.CommandeStatutId == Statuts.FixationDePrixDuTransport))
                .AsQueryable();
            if (clientId.Any())
                query = query.Where(d => clientId.Contains((int)d.IdClient));
            
            if (dateTime.HasValue)
                query = query.Where(d => d.DateCommande.Value.Date == dateTime);

            if (!string.IsNullOrEmpty(dateDebutSearch))query = query.Where(x =>
                x.DateCommande.Value.Date >= DateTime.ParseExact(dateDebutSearch, "dd/MM/yyyy", null).Date );
            
            if(!string.IsNullOrEmpty(dateFinSearch))
                query = query.Where(x =>
                    x.DateCommande.Value.Date <= DateTime.ParseExact(dateFinSearch, "dd/MM/yyyy", null).Date );
            
            return await query
                .Include(d => d.Chantier).ThenInclude(p=>p.Type_Chantier)
                .Include(d => d.Chantier).ThenInclude(p=>p.ZONE_CHANTIER)
                .Include(d => d.Chantier).ThenInclude(p=>p.Centrale_Beton)
                .Include(d => d.Client).ThenInclude(p=>p.Forme_Juridique)
                .Include(d => d.Client).ThenInclude(p=>p.Ville)
                .Include(d => d.Client).ThenInclude(p=>p.Pays)
                .Include(d => d.Statut)
                .Include(p=>p.Tarif_Pompe)
                .Include(d => d.DetailCommandes)
                .ThenInclude(p=>p.Article)
                .Include(d => d.DetailCommandes)
                .ThenInclude(p=>p.Unite)
                .ToListAsync();        }
    }
}
