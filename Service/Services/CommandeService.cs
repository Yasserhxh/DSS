using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using Domain.Models.Commande;
using Microsoft.EntityFrameworkCore.Storage;
using Repository.IRepositories;
using Repository.UnitOfWork;
using Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class CommandeService : ICommandeService
    {
        private readonly ICommandeRepository commandeRepository;
        private readonly IAuthentificationRepository _authentificationRepository;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public CommandeService(ICommandeRepository commandeRepository, IMapper mapper, IUnitOfWork unitOfWork,
            IAuthentificationRepository authentificationRepository)
        {
            this.commandeRepository = commandeRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            _authentificationRepository = authentificationRepository;
        }

        public IEnumerable<FormeJuridiqueModel> GetFormeJuridiques()
        {
            return mapper.Map<IEnumerable<FormeJuridique>, IEnumerable< FormeJuridiqueModel>> (this.commandeRepository.GetFormeJuridiques());
        }
        public IEnumerable<TypeChantierModel> GetTypeChantiers()
        {
            return mapper.Map<IEnumerable<TypeChantier>, IEnumerable<TypeChantierModel>>(this.commandeRepository.GetTypeChantiers());
        }
        public IEnumerable<ZoneModel> GetZones()
        {
            return mapper.Map<IEnumerable<Zone>, IEnumerable<ZoneModel>>(this.commandeRepository.GetZones());
        }
        public IEnumerable<ArticleModel> GetArticles()
        {
            return mapper.Map<IEnumerable<Article>, IEnumerable<ArticleModel>>(this.commandeRepository.GetArticles());
        }
        public IEnumerable<DelaiPaiementModel> GetDelaiPaiements()
        {
            return mapper.Map<IEnumerable<DelaiPaiement>, IEnumerable<DelaiPaiementModel>>(this.commandeRepository.GetDelaiPaiements());
        }
        public IEnumerable<CentraleBetonModel> GetCentraleBetons()
        {
            return mapper.Map<IEnumerable<CentraleBeton>, IEnumerable<CentraleBetonModel>>(this.commandeRepository.GetCentraleBetons());
        }

        public async Task<double> GetTarifArticle(int Id)
        {
            return await commandeRepository.GetTarifArticle(Id);
        }

        public async Task<double> GetTarifZone(int Id)
        {
            return await commandeRepository.GetTarifZone(Id);
        }

        public async Task<double> GetTarifPompe(int Id)
        {
            return await commandeRepository.GetTarifPompe(Id);
        }

        public async Task<bool> CreateCommande (CommandeViewModel commandeViewModel)
        {
            using (IDbContextTransaction transaction = this.unitOfWork.BeginTransaction())
            {
                try
                {
                    // Add chantier
                    Chantier chantier = mapper.Map<ChantierModel, Chantier>(commandeViewModel.Chantier);
                    var ctnId = await commandeRepository.CreateChantier(chantier);

                    // Add client
                    commandeViewModel.Client.Client_Ctn_Id = (int)ctnId;
                    Client client = mapper.Map<ClientModel, Client>(commandeViewModel.Client);
                    var clientId = await commandeRepository.CreateClient(client);

                    // Statut de la commande
                    if (commandeViewModel.DetailCommandes.Any(x => x.IdArticle == 4))
                    {
                        commandeViewModel.Commande.IdStatut = Statuts.EtudeEtPropositionDePrix;
                    }
                    else
                    {
                        var tarifs = await commandeRepository.GetTarifsByArticleIds(commandeViewModel.DetailCommandes.Select(x => x.IdArticle).ToList());
                        foreach (var det in commandeViewModel.DetailCommandes)
                        {
                            if ((double)det.Montant < tarifs[det.IdArticle] || Int64.Parse(commandeViewModel.Commande.Delai_Paiement) > 60)
                            {
                                commandeViewModel.Commande.IdStatut = Statuts.ValidationDeLoffreDePrix;
                                break;
                            }
                        }

                        if (commandeViewModel.Commande.IdStatut != Statuts.ValidationDeLoffreDePrix)
                        {
                            commandeViewModel.Commande.IdStatut = Statuts.FixationDePrixDuTransport;
                        }
                    }

                    // Add commande
                    commandeViewModel.Commande.IdClient = clientId;
                    commandeViewModel.Commande.Currency = "MAD";
                    commandeViewModel.Commande.IdChantier = ctnId;
                    commandeViewModel.Commande.DateCommande = DateTime.Now;
                    List<decimal?> Mt = new List<decimal?>();
                    foreach(var c in commandeViewModel.DetailCommandes)
                    {
                        Mt.Add(c.Volume * c.Montant);
                    }
                    commandeViewModel.Commande.MontantCommande = Mt.Sum();
                    Commande commande = mapper.Map<CommandeModel, Commande>(commandeViewModel.Commande);              
                    var commandeId = await commandeRepository.CreateCommande(commande);

                    // Distinct articles en doublons
                    var details = commandeViewModel.DetailCommandes.GroupBy(x => x.IdArticle, (key,list) => {
                        return new DetailCommandeModel
                        {
                            IdArticle = key,
                            Montant = list.FirstOrDefault().Montant,
                            Volume = list.Sum(x => x.Volume)
                        };
                    }).ToList();

                    // Add details
                    foreach (var detail in details)
                    {
                        detail.IdCommande = commandeId;
                        detail.Unite_Id = 1;
                    }
                    List<DetailCommande> detailCommandes = mapper.Map<List<DetailCommandeModel>, List<DetailCommande>>(details);
                    await commandeRepository.CreateDetailCommande(detailCommandes);

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public async Task<IEnumerable<ClientModel>> GetClients(string Ice = null, string Cnie = null, string RS = null)
        {
            var clients = await commandeRepository.GetClients(Ice, Cnie, RS);
            return mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(clients);
        }

        public async Task<IEnumerable<CommandeModel>> GetCommandes(int? ClientId, DateTime? DateCommande)
        {
            var commandes = await commandeRepository.GetCommandes(ClientId, DateCommande);
            return mapper.Map<IEnumerable<Commande>, IEnumerable<CommandeModel>>(commandes);
        }
        public async Task<IEnumerable<CommandeModel>> GetCommandesPT(int? ClientId, DateTime? DateCommande)
        {
            var commandes = await commandeRepository.GetCommandesPT(ClientId, DateCommande);
            return mapper.Map<IEnumerable<Commande>, IEnumerable<CommandeModel>>(commandes);
        }
        public async Task<CommandeModel> GetCommande(int? id)
        {
            return mapper.Map<CommandeModel>(await commandeRepository.GetCommande(id));
        }

        public IEnumerable<TarifPompeRefModel> GetTarifPompeRefs()
        {
            return mapper.Map<IEnumerable<TarifPompeRef>, IEnumerable<TarifPompeRefModel>>(this.commandeRepository.GetTarifPompeRefs());
        }

        public async Task<bool> ProposerPrix(int Id, decimal Tarif, string UserName)
        {
            using (IDbContextTransaction transaction = this.unitOfWork.BeginTransaction())
            {
                try
                {
                    var detail = await commandeRepository.GetDetailCommande(Id);
                    var user = await _authentificationRepository.GetUserByName(UserName);
                    var userRole = await _authentificationRepository.GetUserRole(user);

                    //Update Detail + Commande
                    detail.Montant = Tarif;
                    detail.Commande.MontantCommande = detail.Commande.MontantCommande + detail.Montant;
                    detail.Commande.IdStatut = Statuts.ValidationDeLoffreDePrix;

                    // Trace Vlidateur
                    ValidationModel validationModel = new ValidationModel()
                    {
                        IdCommande = (int)detail.IdCommande,
                        IdStatut = Statuts.ParametrageDesPrixPBE,
                        Date = DateTime.Now,
                        UserId = user.Id,
                        Nom = user.Nom,
                        Prenom = user.Prenom,
                        Fonction = userRole,
                        ValidationLibelle = "Parametrage des prix PBE"
                    };

                    Validation validation = mapper.Map<ValidationModel, Validation>(validationModel);
                    await commandeRepository.CreateValidation(validation);

                    await unitOfWork.Complete();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public async Task<IEnumerable<CommandeModel>> GetCommandesDAPBE(int? ClientId, DateTime? DateCommande)
        {
            var commandes = await commandeRepository.GetCommandesDAPBE(ClientId, DateCommande);
            return mapper.Map<IEnumerable<Commande>, IEnumerable<CommandeModel>>(commandes);
        }
        public async Task<IEnumerable<CommandeModel>> GetCommandesRC(int? ClientId, DateTime? DateCommande)
        {
            var commandes = await commandeRepository.GetCommandesRC(ClientId, DateCommande);
            return mapper.Map<IEnumerable<Commande>, IEnumerable<CommandeModel>>(commandes);
        }
        public async Task<IEnumerable<CommandeModel>> GetCommandesCV(int? ClientId, DateTime? DateCommande)
        {
            var commandes = await commandeRepository.GetCommandesCV(ClientId, DateCommande);
            return mapper.Map<IEnumerable<Commande>, IEnumerable<CommandeModel>>(commandes);
        }

        public async Task<IEnumerable<CommandeModel>> GetCommandesRL(int? ClientId, DateTime? DateCommande)
        {
            var commandes = await commandeRepository.GetCommandesRL(ClientId, DateCommande);
            return mapper.Map<IEnumerable<Commande>, IEnumerable<CommandeModel>>(commandes);
        }

        public async Task<bool> ProposerPrixDABPE(int Id, decimal Tarif)
        {
            try
            {
                var detail = await commandeRepository.GetDetailCommande(Id);
                detail.Montant = Tarif;
                await unitOfWork.Complete();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RefuserCommande(int Id, string Commentaire)
        {
            try
            {
                var commande = await commandeRepository.GetCommandeOnly(Id);
                commande.IdStatut = null;
                commande.Commentaire = Commentaire;
                await unitOfWork.Complete();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RefuserCommande(int Id, string Commentaire, string UserName)
        {
            using (IDbContextTransaction transaction = this.unitOfWork.BeginTransaction())
            {
                try
                {
                    var commande = await commandeRepository.GetCommandeOnly(Id);
                    var user = await _authentificationRepository.GetUserByName(UserName);
                    var userRole = await _authentificationRepository.GetUserRole(user);

                    //Logique
                    commande.IdStatut = null;
                    commande.Commentaire = Commentaire;

                    //Trace validateur
                    ValidationModel validationModel = new ValidationModel()
                    {
                        IdCommande = Id,
                        IdStatut = Statuts.ValidationDeLoffreDePrix,
                        Date = DateTime.Now,
                        UserId = user.Id,
                        Nom = user.Nom,
                        Prenom = user.Prenom,
                        Fonction = userRole,
                        ValidationLibelle = "Rejet de l'offre de prix"
                    };

                    Validation validation = mapper.Map<ValidationModel, Validation>(validationModel);
                    await commandeRepository.CreateValidation(validation);

                    await unitOfWork.Complete();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public async Task<bool> ValiderCommande(int Id, string Commentaire, string UserName)
        {
            using (IDbContextTransaction transaction = this.unitOfWork.BeginTransaction())
            {
                try
                {
                    var commande = await commandeRepository.GetCommandeOnly(Id);
                    var user = await _authentificationRepository.GetUserByName(UserName);
                    var userRole = await _authentificationRepository.GetUserRole(user);

                    //Update Commande
                    commande.IdStatut = Statuts.FixationDePrixDuTransport;
                    commande.Commentaire = Commentaire;

                    //Trace Validateur
                    ValidationModel validationModel = new ValidationModel()
                    {
                        IdCommande = Id,
                        IdStatut = Statuts.ValidationDeLoffreDePrix,
                        Date = DateTime.Now,
                        UserId = user.Id,
                        Nom = user.Nom,
                        Prenom = user.Prenom,
                        Fonction = userRole,
                        ValidationLibelle = "Validation de l'offre de prix"
                    };

                    Validation validation = mapper.Map<ValidationModel, Validation>(validationModel);
                    await commandeRepository.CreateValidation(validation);

                    await unitOfWork.Complete();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public async Task<bool> UpdateCommande(int id, CommandeViewModel commandeViewModel, string UserName)
        {
            using (IDbContextTransaction transaction = this.unitOfWork.BeginTransaction())
            {
                try
                {
                    var user = await _authentificationRepository.GetUserByName(UserName);
                    var userRole = await _authentificationRepository.GetUserRole(user);
                    var commande = await commandeRepository.GetCommande(id);

                    //Update Commande
                    commande.TarifAchatPompage = commandeViewModel.Commande.TarifAchatPompage;
                    commande.TarifAchatTransport = commandeViewModel.Commande.TarifVenteTransport;
                    commande.Conditions = commandeViewModel.Commande.Conditions;
                    commande.Delai_Paiement = commandeViewModel.Commande.Delai_Paiement;
                    commande.LongFleche_Id = commandeViewModel.Commande.LongFleche_Id;
                    commande.IdStatut = Statuts.ValidationDeLoffreDePrix;
                    List<decimal?> Mt = new List<decimal?>();
                    foreach (var c in commandeViewModel.DetailCommandes)
                    {
                        Mt.Add(c.Volume * c.Montant);
                    }
                    commande.MontantCommande = Mt.Sum();

                    //Update Chantier
                    Chantier chantier = mapper.Map<ChantierModel, Chantier>(commandeViewModel.Chantier);
                    await commandeRepository.UpdateChantier((int)commande.IdChantier, chantier);

                    //Update Client
                    Client client = mapper.Map<ClientModel, Client>(commandeViewModel.Client);
                    await commandeRepository.UpdateClient((int)commande.IdClient, client);

                    //Update Details
                    List<DetailCommande> detailCommandes = mapper.Map<List<DetailCommandeModel>, List<DetailCommande>>(commandeViewModel.DetailCommandes);                  
                    await commandeRepository.UpdateDetailCommande(detailCommandes);

                    //Trace Validateur
                    ValidationModel validationModel = new ValidationModel()
                    {
                        IdCommande = commande.IdCommande,
                        IdStatut = null,
                        Date = DateTime.Now,
                        UserId = user.Id,
                        Nom = user.Nom,
                        Prenom = user.Prenom,
                        Fonction = userRole,
                        ValidationLibelle = "Modification tarif commande"
                    };
                    Validation validation = mapper.Map<ValidationModel, Validation>(validationModel);
                    await commandeRepository.CreateValidation(validation);

                    await unitOfWork.Complete();
                    transaction.Commit();
                    return true;
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public async Task<bool> FixationPrixTransport(int Id, double VenteT, double VenteP)
        {
            try
            {
                var commande = await commandeRepository.GetCommandeOnly(Id);
                commande.TarifVenteTransport = VenteT;
                commande.TarifVentePompage = VenteP;
                commande.IdStatut = Statuts.Validé;
                await unitOfWork.Complete();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
