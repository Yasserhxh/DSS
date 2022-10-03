using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using Domain.Models.Commande;
using Repository.IRepositories;
using Repository.UnitOfWork;
using Service.IServices;

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

        public async Task<List<FormeJuridiqueModel>> GetFormeJuridiques()
        {
            return mapper.Map<List<FormeJuridique>, List< FormeJuridiqueModel>> (await commandeRepository.GetFormeJuridiques());
        }
        public async Task<List<TypeChantierModel>> GetTypeChantiers()
        {
            return mapper.Map<List<TypeChantier>, List<TypeChantierModel>>(await commandeRepository.GetTypeChantiers());
        }
        public async Task<List<ZoneModel>> GetZones()
        {
            return mapper.Map<List<Zone>, List<ZoneModel>>( await this.commandeRepository.GetZones());
        }
        public async Task<List<ArticleModel>> GetArticles()
        {
            return mapper.Map<List<Article>, List<ArticleModel>>(await this.commandeRepository.GetArticles());
        }
        public async Task<List<DelaiPaiementModel>> GetDelaiPaiements()
        {
            return mapper.Map<List<DelaiPaiement>, List<DelaiPaiementModel>>(await this.commandeRepository.GetDelaiPaiements());
        }
        public async Task<List<CentraleBetonModel>> GetCentraleBetons()
        {
            return mapper.Map<List<CentraleBeton>, List<CentraleBetonModel>>(await this.commandeRepository.GetCentraleBetons());
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
            await using var transaction = this.unitOfWork.BeginTransaction();
            try
            {
                // Add chantier
                var chantier = mapper.Map<ChantierModel, Chantier>(commandeViewModel.Chantier);
                var ctnId = await commandeRepository.CreateChantier(chantier);

                // Add client
                commandeViewModel.Client.Client_Ctn_Id = ((int)ctnId);
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
                    foreach (var det in commandeViewModel.DetailCommandes.Where(det => (double)det.Montant < tarifs[det.IdArticle] || long.Parse(commandeViewModel.Commande.Delai_Paiement) > 60))
                    {
                        commandeViewModel.Commande.IdStatut = Statuts.ValidationDeLoffreDePrix;
                        break;
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
                var Mt = commandeViewModel.DetailCommandes.Select(c => c.Volume * c.Montant).ToList();
                commandeViewModel.Commande.MontantCommande = Mt.Sum();
                var commande = mapper.Map<CommandeModel, Commande>(commandeViewModel.Commande);              
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
                var detailCommandes = mapper.Map<List<DetailCommandeModel>, List<DetailCommande>>(details);
                await commandeRepository.CreateDetailCommande(detailCommandes);

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<List<ClientModel>> GetClients(string Ice = null, string Cnie = null, string RS = null)
        {
            var clients = await commandeRepository.GetClients(Ice, Cnie, RS);
            return mapper.Map<List<Client>, List<ClientModel>>(clients);
        }

        public async Task<List<CommandeModel>> GetCommandes(int? ClientId, DateTime? DateCommande)
        {
            var commandes = await commandeRepository.GetCommandes(ClientId, DateCommande);
            return mapper.Map<List<Commande>, List<CommandeModel>>(commandes);
        }
        public async Task<List<CommandeModel>> GetCommandesPT(int? ClientId, DateTime? DateCommande)
        {
            var commandes = await commandeRepository.GetCommandesPT(ClientId, DateCommande);
            return mapper.Map<List<Commande>, List<CommandeModel>>(commandes);
        }
        public async Task<CommandeModel> GetCommande(int? id)
        {
            return mapper.Map<CommandeModel>(await commandeRepository.GetCommande(id));
        }

        public async Task<List<TarifPompeRefModel>> GetTarifPompeRefs()
        {
            return mapper.Map<List<TarifPompeRef>, List<TarifPompeRefModel>>(await this.commandeRepository.GetTarifPompeRefs());
        }

        public async Task<bool> ProposerPrix(int Id, decimal Tarif, string UserName)
        {
            await using var transaction = this.unitOfWork.BeginTransaction();
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

                var validation = mapper.Map<ValidationModel, Validation>(validationModel);
                await commandeRepository.CreateValidation(validation);

                await unitOfWork.Complete();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
        public async Task<List<CommandeModel>> GetCommandesDAPBE(int? ClientId, DateTime? DateCommande)
        {
            var commandes = await commandeRepository.GetCommandesDAPBE(ClientId, DateCommande);
            return mapper.Map<List<Commande>, List<CommandeModel>>(commandes);
        }
        public async Task<List<CommandeModel>> GetCommandesRC(int? ClientId, DateTime? DateCommande)
        {
            var commandes = await commandeRepository.GetCommandesRC(ClientId, DateCommande);
            return mapper.Map<List<Commande>, List<CommandeModel>>(commandes);
        }
        public async Task<List<CommandeModel>> GetCommandesCV(int? ClientId, DateTime? DateCommande)
        {
            var commandes = await commandeRepository.GetCommandesCV(ClientId, DateCommande);
            return mapper.Map<List<Commande>, List<CommandeModel>>(commandes);
        }

        public async Task<List<CommandeModel>> GetCommandesRL(int? ClientId, DateTime? DateCommande)
        {
            var commandes = await commandeRepository.GetCommandesRL(ClientId, DateCommande);
            return mapper.Map<List<Commande>, List<CommandeModel>>(commandes);
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
            await using var transaction = this.unitOfWork.BeginTransaction();
            try
            {
                var commande = await commandeRepository.GetCommandeOnly(Id);
                var user = await _authentificationRepository.GetUserByName(UserName);
                var userRole = await _authentificationRepository.GetUserRole(user);

                //Logique
                commande.IdStatut = null;
                commande.Commentaire = Commentaire;

                //Trace validateur
                var validationModel = new ValidationModel()
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

                var validation = mapper.Map<ValidationModel, Validation>(validationModel);
                await commandeRepository.CreateValidation(validation);

                await unitOfWork.Complete();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> ValiderCommande(int Id, string Commentaire, string UserName)
        {
            await using var transaction = this.unitOfWork.BeginTransaction();
            try
            {
                var commande = await commandeRepository.GetCommandeOnly(Id);
                var user = await _authentificationRepository.GetUserByName(UserName);
                var userRole = await _authentificationRepository.GetUserRole(user);

                //Update Commande
                commande.IdStatut = Statuts.FixationDePrixDuTransport;
                commande.Commentaire = Commentaire;

                //Trace Validateur
                var validationModel = new ValidationModel()
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

                var validation = mapper.Map<ValidationModel, Validation>(validationModel);
                await commandeRepository.CreateValidation(validation);

                await unitOfWork.Complete();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> UpdateCommande(int id, CommandeViewModel commandeViewModel, string UserName)
        {
            await using var transaction = this.unitOfWork.BeginTransaction();
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
                var Mt = commandeViewModel.DetailCommandes.Select(c => c.Volume * c.Montant).ToList();
                commande.MontantCommande = Mt.Sum();

                //Update Chantier
                var chantier = mapper.Map<ChantierModel, Chantier>(commandeViewModel.Chantier);
                await commandeRepository.UpdateChantier((int)commande.IdChantier, chantier);

                //Update Client
                var client = mapper.Map<ClientModel, Client>(commandeViewModel.Client);
                await commandeRepository.UpdateClient((int)commande.IdClient, client);

                //Update Details
                var detailCommandes = mapper.Map<List<DetailCommandeModel>, List<DetailCommande>>(commandeViewModel.DetailCommandes);                  
                await commandeRepository.UpdateDetailCommande(detailCommandes);

                //Trace Validateur
                var validationModel = new ValidationModel()
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
                var validation = mapper.Map<ValidationModel, Validation>(validationModel);
                await commandeRepository.CreateValidation(validation);

                await unitOfWork.Complete();
                await transaction.CommitAsync();
                return true;
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
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
