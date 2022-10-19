using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using Domain.Models.ApiModels;
using Domain.Models.Commande;
using Repository.IRepositories;
using Repository.UnitOfWork;
using Service.IServices;

namespace Service.Services
{
    public class CommandeService : ICommandeService
    {
        private readonly ICommandeRepository _commandeRepository;
        private readonly IAuthentificationRepository _authentificationRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CommandeService(ICommandeRepository commandeRepository, IMapper mapper, IUnitOfWork unitOfWork,
            IAuthentificationRepository authentificationRepository)
        {
            this._commandeRepository = commandeRepository;
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
            _authentificationRepository = authentificationRepository;
        }

        public async Task<List<FormeJuridiqueModel>> GetFormeJuridiques()
        {
            return _mapper.Map<List<FormeJuridique>, List< FormeJuridiqueModel>> (await _commandeRepository.GetFormeJuridiques());
        }
        public async Task<List<TypeChantierModel>> GetTypeChantiers()
        {
            return _mapper.Map<List<TypeChantier>, List<TypeChantierModel>>(await _commandeRepository.GetTypeChantiers());
        }
        public async Task<List<ZoneModel>> GetZones()
        {
            return _mapper.Map<List<Zone>, List<ZoneModel>>( await this._commandeRepository.GetZones());
        }
        public async Task<List<ArticleModel>> GetArticles()
        {
            return _mapper.Map<List<Article>, List<ArticleModel>>(await this._commandeRepository.GetArticles());
        }
        public async Task<List<DelaiPaiementModel>> GetDelaiPaiements()
        {
            return _mapper.Map<List<DelaiPaiement>, List<DelaiPaiementModel>>(await this._commandeRepository.GetDelaiPaiements());
        }
        public async Task<List<CentraleBetonModel>> GetCentraleBetons()
        {
            return _mapper.Map<List<CentraleBeton>, List<CentraleBetonModel>>(await this._commandeRepository.GetCentraleBetons());
        }

        public async Task<double> GetTarifArticle(int Id)
        {
            return await _commandeRepository.GetTarifArticle(Id);
        }

        public async Task<double> GetTarifZone(int Id)
        {
            return await _commandeRepository.GetTarifZone(Id);
        }

        public async Task<double> GetTarifPompe(int Id)
        {
            return await _commandeRepository.GetTarifPompe(Id);
        }
        public async Task<List<VilleModel>> GetVilles() => _mapper.Map<List<Ville>, List<VilleModel>> (await _commandeRepository.GetVilles());
        public async Task<List<PaysModel>> GetPays() => _mapper.Map<List<Pays>, List<PaysModel>> (await _commandeRepository.GetPays());
        public async Task<bool> CreateCommande (CommandeViewModel commandeViewModel)
        {
            await using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                // Add chantier
                var chantier = _mapper.Map<ChantierModel, Chantier>(commandeViewModel.Chantier);
                var ctnId = await _commandeRepository.CreateChantier(chantier);

                // Add client
                commandeViewModel.Client.Client_Ctn_Id = (int)ctnId;
                var result = _commandeRepository.FindFormulaireClient(commandeViewModel.Client.Ice,
                        commandeViewModel.Client.Cnie);
                int? clientId;
                if (result == null)
                {
                    var client = _mapper.Map<ClientModel, Client>(commandeViewModel.Client);
                     clientId = await _commandeRepository.CreateClient(client);
                }
                else
                {
                    var client = _mapper.Map<ClientModel, Client>(commandeViewModel.Client);
                    var resUpdate = await _commandeRepository.UpdateClient(result.Client_Id, client);
                    if(!resUpdate)
                        return false;
                    clientId = result.Client_Id;

                }
                
                
                // Statut de la commande
                
                if (commandeViewModel.Commande.TarifAchatTransport !=  0)
                {
                    commandeViewModel.Commande.IdStatut = Statuts.EnCoursDeTraitement;
                    commandeViewModel.Commande.CommandeStatuts.Add(new CommandeStatutModel
                    {
                        StatutId = Statuts.FixationDePrixDuTransport, 
                    });
                }
                
                
                if (commandeViewModel.DetailCommandes.Any(x => x.IdArticle == 4))
                {
                    commandeViewModel.Commande.IdStatut = Statuts.EnCoursDeTraitement;
                    commandeViewModel.Commande.CommandeStatuts.Add(new CommandeStatutModel
                    {
                        StatutId = Statuts.EtudeEtPropositionDePrix 
                    });
                }
                
                var tarifs = await _commandeRepository.GetTarifsByArticleIds(commandeViewModel.DetailCommandes.Select(x => x.IdArticle).ToList());
                if (commandeViewModel.DetailCommandes.Any(det =>tarifs[det.IdArticle] - (double)det.Montant  > 10 || long.Parse(commandeViewModel.Commande.Delai_Paiement) > 60))
                {
                    commandeViewModel.Commande.IdStatut = Statuts.EnCoursDeTraitement;
                    commandeViewModel.Commande.CommandeStatuts.Add(new CommandeStatutModel
                    {
                        StatutId = Statuts.ValidationDeLoffreDePrixDABPE
                    });
                }
                if (commandeViewModel.DetailCommandes.Any(det => tarifs[det.IdArticle] - (double)det.Montant  == 10))
                {
                    commandeViewModel.Commande.IdStatut = Statuts.EnCoursDeTraitement;
                    commandeViewModel.Commande.CommandeStatuts.Add(new CommandeStatutModel
                    {
                        StatutId = Statuts.ValidationDeLoffreDePrixRC
                    });
                }
                if (commandeViewModel.DetailCommandes.Any(det => tarifs[det.IdArticle] - (double)det.Montant  >= 5 && tarifs[det.IdArticle] - (double)det.Montant  <10))
                {
                    commandeViewModel.Commande.IdStatut = Statuts.EnCoursDeTraitement;
                    commandeViewModel.Commande.CommandeStatuts.Add(new CommandeStatutModel
                    {
                        StatutId = Statuts.ValidationDeLoffreDePrixCV
                    });
                }

                //var statuts = new List<int>{1,3,41};
                if (commandeViewModel.Commande.IdStatut is null)
                {
                    commandeViewModel.Commande.IdStatut = Statuts.Validé;
                    commandeViewModel.Commande.CommandeStatuts.Add(new CommandeStatutModel
                    {
                        StatutId = Statuts.Validé 
                    });
                }
                

                // Add commande
                commandeViewModel.Commande.IdClient = clientId;
                commandeViewModel.Commande.Currency = "MAD";
                commandeViewModel.Commande.IdChantier = ctnId;
                commandeViewModel.Commande.DateCommande = DateTime.Now;
                var Mt = commandeViewModel.DetailCommandes.Select(c => c.Volume * c.Montant).ToList();
                commandeViewModel.Commande.MontantCommande = Mt.Sum();
                var commande = _mapper.Map<CommandeModel, Commande>(commandeViewModel.Commande);              
                var commandeId = await _commandeRepository.CreateCommande(commande);

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
                var detailCommandes = _mapper.Map<List<DetailCommandeModel>, List<DetailCommande>>(details);
                await _commandeRepository.CreateDetailCommande(detailCommandes);

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<List<ClientModel>> GetClients(string Ice = null, string Cnie = null, string RS = null)
        {
            var clients = await _commandeRepository.GetClients(Ice, Cnie, RS);
            return _mapper.Map<List<Client>, List<ClientModel>>(clients);
        }

        public async Task<List<CommandeApiModel>> GetCommandes(List<int> ClientId, DateTime? DateCommande)
        {
            var commandes = await _commandeRepository.GetCommandes(ClientId, DateCommande);
            var listDetailCommandeApi = new List<DetailCommandeApiModel>();
            //return mapper.Map<List<Commande>, List<CommandeModel>>(commandes);
            return commandes.Select(item => new CommandeApiModel
                {
                    CommandeId = item.IdCommande,
                    CodeCommandeSap = item.CodeClientSap,
                    StatutCommande = item.Statut.Libelle,
                    DateCommande = item.DateCommande,
                    DateLivraisonSouhaite = item.DateLivraisonSouhaite,
                    TarifAchatTransport = item.TarifAchatTransport,
                    TarifVenteTransport = item.TarifVenteTransport,
                    TarifAchatPompage = item.TarifAchatPompage,
                    TarifVentePompage = item.TarifVentePompage,
                    Conditions = item.Conditions,
                    DelaiPaiement = item.Delai_Paiement,
                    //LongFlecheLibelle = item.Tarif_Pompe.LongFleche_Libelle,
                    //LongFlechePrix = item.Tarif_Pompe.LongFleche_Prix,
                    Commentaire = item.Commentaire,
                    ArticleFile = item.ArticleFile,
                    Ice = item.Client.Ice,
                    Cnie = item.Client.Cnie,
                    FormeJuridique = item.Client.Forme_Juridique.FormeJuridique_Libelle,
                    RaisonSociale = item.Client.RaisonSociale,
                    CtnNom = item.Chantier.Ctn_Nom,
                    CtnType = item.Chantier.Type_Chantier.Tc_Libelle,
                    CtnZone = item.Chantier.ZONE_CHANTIER.Zone_Libelle,
                    MaitreOuvrage = item.Chantier.MaitreOuvrage,
                    VolumePrevisonnel = item.Chantier.VolumePrevisonnel,
                    Duree = item.Chantier.Duree,
                    Rayon = item.Chantier.Rayon,
                    CtrNom = item.Chantier.Centrale_Beton.Ctr_Nom,
                    Gsm = item.Client.Gsm,
                    Adresse = item.Client.Adresse,
                    Email = item.Client.Email,
                    DestinataireInterlocuteur = item.Client.Destinataire_Interlocuteur,
                    Ville = item.Client.Ville.NomVille,
                    Pays = item.Client.Pays.NomPays
                    // DetailsCommande  = listDetailCommandeApi
                })
                .ToList();
        }
        public async Task<List<CommandeApiModel>> GetCommandesPT(List<int>  ClientId, DateTime? DateCommande, string DateDebutSearch, string DateFinSearch)
        {
            var commandes = await _commandeRepository.GetCommandesPT(ClientId, DateCommande,DateDebutSearch,DateFinSearch);
            var commandesApi = new List<CommandeApiModel>();
            var listDetailCommandeApi = new List<DetailCommandeApiModel>();
            foreach (var item in commandes)
            {
                /*listDetailCommandeApi.AddRange(item.DetailCommandes.Select(detail => new DetailCommandeApiModel
                {
                    IdDetailCommande = detail.IdDetailCommande,
                    ArticleDesignation = detail.Article.Designation,
                    Montant = detail.Montant,
                    DateProduction = detail.DateProduction,
                    Volume = detail.Volume,
                    UniteLibelle = detail.Unite.Libelle
                }));*/
                var commandeApi = new CommandeApiModel
                {
                     CommandeId = item.IdCommande,
                    CodeCommandeSap = item.CodeClientSap,
                    StatutCommande = item.Statut.Libelle,
                    DateCommande = item.DateCommande,
                    DateLivraisonSouhaite = item.DateLivraisonSouhaite,
                    TarifAchatTransport = item.TarifAchatTransport,
                    TarifVenteTransport = item.TarifVenteTransport,
                    TarifAchatPompage = item.TarifAchatPompage,
                    TarifVentePompage = item.TarifVentePompage,
                    Conditions = item.Conditions,
                    DelaiPaiement = item.Delai_Paiement,
                    //LongFlecheLibelle = item.Tarif_Pompe.LongFleche_Libelle,
                    //LongFlechePrix = item.Tarif_Pompe.LongFleche_Prix,
                    Commentaire = item.Commentaire,
                    ArticleFile = item.ArticleFile,
                    Ice = item.Client.Ice,
                    Cnie = item.Client.Cnie,
                    FormeJuridique = item.Client.Forme_Juridique.FormeJuridique_Libelle,
                    RaisonSociale = item.Client.RaisonSociale,
                    CtnNom = item.Chantier.Ctn_Nom,
                    CtnType = item.Chantier.Type_Chantier.Tc_Libelle,
                    CtnZone = item.Chantier.ZONE_CHANTIER.Zone_Libelle,
                    MaitreOuvrage = item.Chantier.MaitreOuvrage,
                    VolumePrevisonnel = item.Chantier.VolumePrevisonnel,
                    Duree = item.Chantier.Duree,
                    Rayon = item.Chantier.Rayon,
                    CtrNom = item.Chantier.Centrale_Beton.Ctr_Nom,
                    Gsm = item.Client.Gsm,
                    Adresse = item.Client.Adresse,
                    Email = item.Client.Email,
                    DestinataireInterlocuteur = item.Client.Destinataire_Interlocuteur,
                    Ville = item.Client.Ville.NomVille,
                    Pays = item.Client.Pays.NomPays
                    // DetailsCommande  = listDetailCommandeApi
                };
                commandesApi.Add(commandeApi);
            }
            //return mapper.Map<List<Commande>, List<CommandeModel>>(commandes);
            return commandesApi;
            // mapper.Map<List<Commande>, List<CommandeModel>>(commandes);
        }
        public async Task<CommandeModel> GetCommande(int? id)
        {
            return _mapper.Map<CommandeModel>(await _commandeRepository.GetCommande(id));
        }

        public async Task<List<TarifPompeRefModel>> GetTarifPompeRefs()
        {
            return _mapper.Map<List<TarifPompeRef>, List<TarifPompeRefModel>>(await this._commandeRepository.GetTarifPompeRefs());
        }
        public async Task<List<DetailCommandeApiModel>> GetCommandesDetails(int? commandeId)
        {
            var commandes = await _commandeRepository.GetListDetailsCommande(commandeId);
            List<DetailCommandeApiModel> listDetailsApiModel = new();
            foreach (var item in commandes)
            {
                var detailcommandeApi = new DetailCommandeApiModel
                {
                    IdDetailCommande = item.IdDetailCommande,
                    ArticleDesignation = item.Article.Designation,
                    Montant = item.Montant,
                    DateProduction = item.DateProduction,
                    Volume = item.Volume,
                    UniteLibelle = item.Unite.Libelle
                    
                };
                listDetailsApiModel.Add(detailcommandeApi);
            }
            //return mapper.Map<List<DetailCommande>, List<DetailCommandeModel>>(commandes);
            return listDetailsApiModel;
        }

        public async Task<List<StatutModel>> GetCommandesStatuts(int? id) =>
            _mapper.Map<List<Statut>, List<StatutModel>>(
                await _commandeRepository.GetCommandesStatuts(id));
        public async Task<bool> ProposerPrix(int Id, decimal Tarif, string UserName)
        {
            await using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var detail = await _commandeRepository.GetDetailCommande(Id);
                var user = await _authentificationRepository.GetUserByName(UserName);
                var userRole = await _authentificationRepository.GetUserRole(user);

                //Update Detail + Commande
                detail.Montant = Tarif;
                detail.Commande.MontantCommande = detail.Commande.MontantCommande + detail.Montant;
               // detail.Commande.IdStatut = Statuts.ValidationDeLoffreDePrix;

                // Trace Vlidateur
                var validationModel = new ValidationModel
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

                var validation = _mapper.Map<ValidationModel, Validation>(validationModel);
                await _commandeRepository.CreateValidation(validation);

                await _unitOfWork.Complete();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
        public async Task<List<CommandeApiModel>> GetCommandesDAPBE(List<int> ClientId, DateTime? DateCommande, string dateDebutSearch, string dateFinSearch)
        {
            var commandes = await _commandeRepository.GetCommandesDAPBE(ClientId, DateCommande , dateDebutSearch,dateFinSearch);
                        var commandesApi = new List<CommandeApiModel>();
            var listDetailCommandeApi = new List<DetailCommandeApiModel>();
            foreach (var item in commandes)
            {
               /*listDetailCommandeApi.AddRange(item.DetailCommandes.Select(detail => new DetailCommandeApiModel
                {
                    IdDetailCommande = detail.IdDetailCommande,
                    ArticleDesignation = detail.Article.Designation,
                    Montant = detail.Montant,
                    DateProduction = detail.DateProduction,
                    Volume = detail.Volume,
                    UniteLibelle = detail.Unite.Libelle
                }));*/
                var commandeApi = new CommandeApiModel
                {
                      CommandeId = item.IdCommande,
                    CodeCommandeSap = item.CodeClientSap,
                    StatutCommande = item.Statut.Libelle,
                    DateCommande = item.DateCommande,
                    DateLivraisonSouhaite = item.DateLivraisonSouhaite,
                    TarifAchatTransport = item.TarifAchatTransport,
                    TarifVenteTransport = item.TarifVenteTransport,
                    TarifAchatPompage = item.TarifAchatPompage,
                    TarifVentePompage = item.TarifVentePompage,
                    Conditions = item.Conditions,
                    DelaiPaiement = item.Delai_Paiement,
                    //LongFlecheLibelle = item.Tarif_Pompe.LongFleche_Libelle,
                    //LongFlechePrix = item.Tarif_Pompe.LongFleche_Prix,
                    Commentaire = item.Commentaire,
                    ArticleFile = item.ArticleFile,
                    Ice = item.Client.Ice,
                    Cnie = item.Client.Cnie,
                    FormeJuridique = item.Client.Forme_Juridique.FormeJuridique_Libelle,
                    RaisonSociale = item.Client.RaisonSociale,
                    CtnNom = item.Chantier.Ctn_Nom,
                    CtnType = item.Chantier.Type_Chantier.Tc_Libelle,
                    CtnZone = item.Chantier.ZONE_CHANTIER.Zone_Libelle,
                    MaitreOuvrage = item.Chantier.MaitreOuvrage,
                    VolumePrevisonnel = item.Chantier.VolumePrevisonnel,
                    Duree = item.Chantier.Duree,
                    Rayon = item.Chantier.Rayon,
                    CtrNom = item.Chantier.Centrale_Beton.Ctr_Nom,
                    Gsm = item.Client.Gsm,
                    Adresse = item.Client.Adresse,
                    Email = item.Client.Email,
                    DestinataireInterlocuteur = item.Client.Destinataire_Interlocuteur,
                    Ville = item.Client.Ville.NomVille,
                    Pays = item.Client.Pays.NomPays
                    // DetailsCommande  = listDetailCommandeApi
                };
                commandesApi.Add(commandeApi);
            }
            //return mapper.Map<List<Commande>, List<CommandeModel>>(commandes);
            return commandesApi;
            //return mapper.Map<List<Commande>, List<CommandeModel>>(commandes);
        }
        public async Task<List<CommandeApiModel>> GetCommandesRC(List<int> ClientId, DateTime? DateCommande, string dateDebutSearch, string dateFinSearch)
        {
            var commandes = await _commandeRepository.GetCommandesRC(ClientId, DateCommande, dateDebutSearch,dateFinSearch);
                        var commandesApi = new List<CommandeApiModel>();
            var listDetailCommandeApi = new List<DetailCommandeApiModel>();
            foreach (var item in commandes)
            {
                /*listDetailCommandeApi.AddRange(item.DetailCommandes.Select(detail => new DetailCommandeApiModel
                {
                    IdDetailCommande = detail.IdDetailCommande,
                    ArticleDesignation = detail.Article.Designation,
                    Montant = detail.Montant,
                    DateProduction = detail.DateProduction,
                    Volume = detail.Volume,
                    UniteLibelle = detail.Unite.Libelle
                }));*/
                var commandeApi = new CommandeApiModel
                { 
                    CommandeId = item.IdCommande,
                    CodeCommandeSap = item.CodeClientSap,
                    StatutCommande = item.Statut.Libelle,
                    DateCommande = item.DateCommande,
                    DateLivraisonSouhaite = item.DateLivraisonSouhaite,
                    TarifAchatTransport = item.TarifAchatTransport,
                    TarifVenteTransport = item.TarifVenteTransport,
                    TarifAchatPompage = item.TarifAchatPompage,
                    TarifVentePompage = item.TarifVentePompage,
                    Conditions = item.Conditions,
                    DelaiPaiement = item.Delai_Paiement,
                    //LongFlecheLibelle = item.Tarif_Pompe.LongFleche_Libelle,
                    //LongFlechePrix = item.Tarif_Pompe.LongFleche_Prix,
                    Commentaire = item.Commentaire,
                    ArticleFile = item.ArticleFile,
                    Ice = item.Client.Ice,
                    Cnie = item.Client.Cnie,
                    FormeJuridique = item.Client.Forme_Juridique.FormeJuridique_Libelle,
                    RaisonSociale = item.Client.RaisonSociale,
                    CtnNom = item.Chantier.Ctn_Nom,
                    CtnType = item.Chantier.Type_Chantier.Tc_Libelle,
                    CtnZone = item.Chantier.ZONE_CHANTIER.Zone_Libelle,
                    MaitreOuvrage = item.Chantier.MaitreOuvrage,
                    VolumePrevisonnel = item.Chantier.VolumePrevisonnel,
                    Duree = item.Chantier.Duree,
                    Rayon = item.Chantier.Rayon,
                    CtrNom = item.Chantier.Centrale_Beton.Ctr_Nom,
                    Gsm = item.Client.Gsm,
                    Adresse = item.Client.Adresse,
                    Email = item.Client.Email,
                    DestinataireInterlocuteur = item.Client.Destinataire_Interlocuteur,
                    Ville = item.Client.Ville.NomVille,
                    Pays = item.Client.Pays.NomPays
                    // DetailsCommande  = listDetailCommandeApi
                };
                commandesApi.Add(commandeApi);
            }
            //return mapper.Map<List<Commande>, List<CommandeModel>>(commandes);
            return commandesApi;
            //return mapper.Map<List<Commande>, List<CommandeModel>>(commandes);
        }
        public async Task<List<CommandeApiModel>> GetCommandesCV(List<int>  ClientId, DateTime? DateCommande, string dateDebutSearch, string dateFinSrearch)
        {
            var commandes = await _commandeRepository.GetCommandesCV(ClientId, DateCommande, dateDebutSearch, dateFinSrearch);
                        var commandesApi = new List<CommandeApiModel>();
            var listDetailCommandeApi = new List<DetailCommandeApiModel>();
            foreach (var item in commandes)
            {
                /*listDetailCommandeApi.AddRange(item.DetailCommandes.Select(detail => new DetailCommandeApiModel
                {
                    IdDetailCommande = detail.IdDetailCommande,
                    ArticleDesignation = detail.Article.Designation,
                    Montant = detail.Montant,
                    DateProduction = detail.DateProduction,
                    Volume = detail.Volume,
                    UniteLibelle = detail.Unite.Libelle
                }));*/
                var commandeApi = new CommandeApiModel
                {
                      CommandeId = item.IdCommande,
                    CodeCommandeSap = item.CodeClientSap,
                    StatutCommande = item.Statut.Libelle,
                    DateCommande = item.DateCommande,
                    DateLivraisonSouhaite = item.DateLivraisonSouhaite,
                    TarifAchatTransport = item.TarifAchatTransport,
                    TarifVenteTransport = item.TarifVenteTransport,
                    TarifAchatPompage = item.TarifAchatPompage,
                    TarifVentePompage = item.TarifVentePompage,
                    Conditions = item.Conditions,
                    DelaiPaiement = item.Delai_Paiement,
                    //LongFlecheLibelle = item.Tarif_Pompe.LongFleche_Libelle,
                    //LongFlechePrix = item.Tarif_Pompe.LongFleche_Prix,
                    Commentaire = item.Commentaire,
                    ArticleFile = item.ArticleFile,
                    Ice = item.Client.Ice,
                    Cnie = item.Client.Cnie,
                    FormeJuridique = item.Client.Forme_Juridique.FormeJuridique_Libelle,
                    RaisonSociale = item.Client.RaisonSociale,
                    CtnNom = item.Chantier.Ctn_Nom,
                    CtnType = item.Chantier.Type_Chantier.Tc_Libelle,
                    CtnZone = item.Chantier.ZONE_CHANTIER.Zone_Libelle,
                    MaitreOuvrage = item.Chantier.MaitreOuvrage,
                    VolumePrevisonnel = item.Chantier.VolumePrevisonnel,
                    Duree = item.Chantier.Duree,
                    Rayon = item.Chantier.Rayon,
                    CtrNom = item.Chantier.Centrale_Beton.Ctr_Nom,
                    Gsm = item.Client.Gsm,
                    Adresse = item.Client.Adresse,
                    Email = item.Client.Email,
                    DestinataireInterlocuteur = item.Client.Destinataire_Interlocuteur,
                    Ville = item.Client.Ville.NomVille,
                    Pays = item.Client.Pays.NomPays
                    // DetailsCommande  = listDetailCommandeApi
                };
                commandesApi.Add(commandeApi);
            }
            //return mapper.Map<List<Commande>, List<CommandeModel>>(commandes);
            return commandesApi;
            //return mapper.Map<List<Commande>, List<CommandeModel>>(commandes);
        }

        public async Task<List<CommandeApiModel>> GetCommandesRL(List<int> ClientIds, DateTime? DateCommande, string DateDebutSearch, string DateFinSearch)
        {
            var commandes = await _commandeRepository.GetCommandesRL(ClientIds, DateCommande,DateDebutSearch,DateFinSearch);
                        var commandesApi = new List<CommandeApiModel>();
            var listDetailCommandeApi = new List<DetailCommandeApiModel>();
            foreach (var item in commandes)
            {
                /*listDetailCommandeApi.AddRange(item.DetailCommandes.Select(detail => new DetailCommandeApiModel
                {
                    IdDetailCommande = detail.IdDetailCommande,
                    ArticleDesignation = detail.Article.Designation,
                    Montant = detail.Montant,
                    DateProduction = detail.DateProduction,
                    Volume = detail.Volume,
                    UniteLibelle = detail.Unite.Libelle
                }));*/
                var commandeApi = new CommandeApiModel
                {
                   CommandeId = item.IdCommande,
                    CodeCommandeSap = item.CodeClientSap,
                    StatutCommande = item.Statut.Libelle,
                    DateCommande = item.DateCommande,
                    DateLivraisonSouhaite = item.DateLivraisonSouhaite,
                    TarifAchatTransport = item.TarifAchatTransport,
                    TarifVenteTransport = item.TarifVenteTransport,
                    TarifAchatPompage = item.TarifAchatPompage,
                    TarifVentePompage = item.TarifVentePompage,
                    Conditions = item.Conditions,
                    DelaiPaiement = item.Delai_Paiement,
                    //LongFlecheLibelle = item.Tarif_Pompe.LongFleche_Libelle,
                    //LongFlechePrix = item.Tarif_Pompe.LongFleche_Prix,
                    Commentaire = item.Commentaire,
                    ArticleFile = item.ArticleFile,
                    Ice = item.Client.Ice,
                    Cnie = item.Client.Cnie,
                    FormeJuridique = item.Client.Forme_Juridique.FormeJuridique_Libelle,
                    RaisonSociale = item.Client.RaisonSociale,
                    CtnNom = item.Chantier.Ctn_Nom,
                    CtnType = item.Chantier.Type_Chantier.Tc_Libelle,
                    CtnZone = item.Chantier.ZONE_CHANTIER.Zone_Libelle,
                    MaitreOuvrage = item.Chantier.MaitreOuvrage,
                    VolumePrevisonnel = item.Chantier.VolumePrevisonnel,
                    Duree = item.Chantier.Duree,
                    Rayon = item.Chantier.Rayon,
                    CtrNom = item.Chantier.Centrale_Beton.Ctr_Nom,
                    Gsm = item.Client.Gsm,
                    Adresse = item.Client.Adresse,
                    Email = item.Client.Email,
                    DestinataireInterlocuteur = item.Client.Destinataire_Interlocuteur,
                    Ville = item.Client.Ville.NomVille,
                    Pays = item.Client.Pays.NomPays
                    // DetailsCommande  = listDetailCommandeApi
                };
                commandesApi.Add(commandeApi);
            }
            //return mapper.Map<List<Commande>, List<CommandeModel>>(commandes);
            return commandesApi;
            //return mapper.Map<List<Commande>, List<CommandeModel>>(commandes);
        }

        public async Task<bool> ProposerPrixDABPE(int Id, decimal Tarif)
        {
            try
            {
                var detail = await _commandeRepository.GetDetailCommande(Id);
                detail.Montant = Tarif;
                await _unitOfWork.Complete();
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
                var commande = await _commandeRepository.GetCommandeOnly(Id);
                commande.IdStatut = null;
                commande.Commentaire = Commentaire;
                await _unitOfWork.Complete();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RefuserCommande(int Id, string Commentaire, string UserName)
        {
            await using var transaction = this._unitOfWork.BeginTransaction();
            try
            {
                var commande = await _commandeRepository.GetCommandeOnly(Id);
                var user = await _authentificationRepository.GetUserByName(UserName);
                var userRole = await _authentificationRepository.GetUserRole(user);

                //Logique
                commande.IdStatut = null;
                commande.Commentaire = Commentaire;

                //Trace validateur
                var validationModel = new ValidationModel()
                {
                    IdCommande = Id,
                  //  IdStatut = Statuts.ValidationDeLoffreDePrix,
                    Date = DateTime.Now,
                    UserId = user.Id,
                    Nom = user.Nom,
                    Prenom = user.Prenom,
                    Fonction = userRole,
                    ValidationLibelle = "Rejet de l'offre de prix"
                };

                var validation = _mapper.Map<ValidationModel, Validation>(validationModel);
                await _commandeRepository.CreateValidation(validation);

                await _unitOfWork.Complete();
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
            await using var transaction = this._unitOfWork.BeginTransaction();
            try
            {
                var commande = await _commandeRepository.GetCommandeOnly(Id);
                var user = await _authentificationRepository.GetUserByName(UserName);
                var userRole = await _authentificationRepository.GetUserRole(user);

                //Update Commande
                commande.IdStatut = Statuts.FixationDePrixDuTransport;
                commande.Commentaire = Commentaire;

                //Trace Validateur
                var validationModel = new ValidationModel()
                {
                    IdCommande = Id,
                //    IdStatut = Statuts.ValidationDeLoffreDePrix,
                    Date = DateTime.Now,
                    UserId = user.Id,
                    Nom = user.Nom,
                    Prenom = user.Prenom,
                    Fonction = userRole,
                    ValidationLibelle = "Validation de l'offre de prix"
                };

                var validation = _mapper.Map<ValidationModel, Validation>(validationModel);
                await _commandeRepository.CreateValidation(validation);

                await _unitOfWork.Complete();
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
            await using var transaction = this._unitOfWork.BeginTransaction();
            try
            {
                var user = await _authentificationRepository.GetUserByName(UserName);
                var userRole = await _authentificationRepository.GetUserRole(user);
                var commande = await _commandeRepository.GetCommande(id);

                //Update Commande
                commande.TarifAchatPompage = commandeViewModel.Commande.TarifAchatPompage;
                commande.TarifAchatTransport = commandeViewModel.Commande.TarifVenteTransport;
                commande.Conditions = commandeViewModel.Commande.Conditions;
                commande.Delai_Paiement = commandeViewModel.Commande.Delai_Paiement;
                commande.LongFleche_Id = commandeViewModel.Commande.LongFleche_Id;
               // commande.IdStatut = Statuts.ValidationDeLoffreDePrix;
                var Mt = commandeViewModel.DetailCommandes.Select(c => c.Volume * c.Montant).ToList();
                commande.MontantCommande = Mt.Sum();

                //Update Chantier
                var chantier = _mapper.Map<ChantierModel, Chantier>(commandeViewModel.Chantier);
                await _commandeRepository.UpdateChantier((int)commande.IdChantier, chantier);

                //Update Client
                var client = _mapper.Map<ClientModel, Client>(commandeViewModel.Client);
                await _commandeRepository.UpdateClient((int)commande.IdClient, client);

                //Update Details
                var detailCommandes = _mapper.Map<List<DetailCommandeModel>, List<DetailCommande>>(commandeViewModel.DetailCommandes);                  
                await _commandeRepository.UpdateDetailCommande(detailCommandes);

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
                var validation = _mapper.Map<ValidationModel, Validation>(validationModel);
                await _commandeRepository.CreateValidation(validation);

                await _unitOfWork.Complete();
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
                var commande = await _commandeRepository.GetCommandeOnly(Id);
                commande.TarifVenteTransport = VenteT;
                commande.TarifVentePompage = VenteP;
                commande.IdStatut = Statuts.Validé;
                await _unitOfWork.Complete();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public ClientModel FindFormulaireClient(string Ice, string Cnie)
        {
            var result =  _commandeRepository.FindFormulaireClient(Ice, Cnie);
            return _mapper.Map<Client,ClientModel>(result);
        }
    }
}
