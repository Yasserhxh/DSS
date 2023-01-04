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

        public async Task<bool> CreateProspect(CommandeViewModel commandeViewModel)
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
                    commandeViewModel.Client.Cnie, commandeViewModel.Client.RaisonSociale, null);
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
                
                //Add Prospect
                var prospect = new Prospect
                {
                    IdChantier = ctnId,
                    IdClient = clientId,
                    //CodeClientSap = client
                    DateProspect = DateTime.Now,
                    CheckOffre = false
                };
                var prospectId = await _commandeRepository.CreateProspect(prospect);
                await transaction.CommitAsync();
                return prospectId != null;
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<List<string>> CreateCommande (CommandeViewModel commandeViewModel)
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
                        commandeViewModel.Client.Cnie, commandeViewModel.Client.RaisonSociale, null);
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
                        return null;
                    clientId = result.Client_Id;

                }
                
                //Add Prospect
                var prospect = new Prospect
                {
                    IdChantier = ctnId,
                    IdClient = clientId,
                    //CodeClientSap = client
                    DateProspect = DateTime.Now,
                    CheckOffre = true

                };
                var prospectId = await _commandeRepository.CreateProspect(prospect);
                
                // Statut de la commande
                if (commandeViewModel.Commande.IsProspection)
                {
                    if (commandeViewModel.Commande.TarifAchatTransport !=  0)
                    {
                        var email = _authentificationRepository.FindUserByEmailByRoleAndRegion("Responsable logistique",
                            1).Result.Email;                        
                        commandeViewModel.Commande.Emails.Add(email);

                        commandeViewModel.Commande.IdStatut = Statuts.EnCoursDeTraitement;
                        commandeViewModel.Commande.CommandeStatuts.Add(new CommandeStatutModel
                        {
                            StatutId = Statuts.FixationDePrixDuTransport, 
                        });
                    }
                
                
                    if (commandeViewModel.DetailCommandes.Any(x => x.IdArticle == 14))
                    {
                        var email = _authentificationRepository.FindUserByEmailByRoleAndRegion("Prescripteur technique",
                            1).Result.Email;                        
                        commandeViewModel.Commande.Emails.Add(email);
                       // var email2 = _authentificationRepository.FindUserByEmailByRoleAndRegion("Responsable commercial",
                        //    1).Result.Email;                        
                       // commandeViewModel.Commande.Emails.Add(email2);
                        
                        commandeViewModel.Commande.IdStatut = Statuts.EnCoursDeTraitement;
                        commandeViewModel.Commande.CommandeStatuts.Add(new CommandeStatutModel
                        {
                            StatutId = Statuts.EtudeEtPropositionDePrix

                        }); 
                        /*  commandeViewModel.Commande.CommandeStatuts.Add(new CommandeStatutModel
                          {
                              StatutId = Statuts.ValidationDeLoffreDePrixDABPE
      
                          });*/
                        /*commandeViewModel.Commande.CommandeStatuts.Add(new CommandeStatutModel
                        {
                            StatutId = Statuts.ValidationDeLoffreDePrixRC

                        });*/
                    }

                
                
                    var tarifs = await _commandeRepository.GetTarifsByArticleIds(commandeViewModel.DetailCommandes.Select(x => x.IdArticle).ToList());
                    if (commandeViewModel.DetailCommandes.Any(det =>tarifs[det.IdArticle] - (double)det.Montant  > 10 /*|| long.Parse(commandeViewModel.Commande.Delai_Paiement) > 60*/))
                    {
                        commandeViewModel.Commande.Emails.Add(_authentificationRepository.FindUserByEmailByRoleAndRegion("Chef de ventes",
                            1).Result.Email);
                        commandeViewModel.Commande.Emails.Add(_authentificationRepository.FindUserByEmailByRoleAndRegion("Responsable commercial",
                            1).Result.Email); 
                        commandeViewModel.Commande.Emails.Add( _authentificationRepository.FindUserByEmailByRoleAndRegion("DA BPE",
                            1).Result.Email);
                        
                       
                        
                        
                        
                        commandeViewModel.Commande.IdStatut = Statuts.EnCoursDeTraitement;
                        commandeViewModel.Commande.CommandeStatuts.Add(new CommandeStatutModel
                        {
                            StatutId = Statuts.ValidationDeLoffreDePrixCV
                        });
                        commandeViewModel.Commande.CommandeStatuts.Add(new CommandeStatutModel
                        {
                            StatutId = Statuts.ValidationDeLoffreDePrixRC
                        });
                        commandeViewModel.Commande.CommandeStatuts.Add(new CommandeStatutModel
                        {
                            StatutId = Statuts.ValidationDeLoffreDePrixDABPE
                        });
                    }
                    else if (commandeViewModel.DetailCommandes.Any(det => tarifs[det.IdArticle] - (double)det.Montant  > 5 && tarifs[det.IdArticle] - (double)det.Montant  <=10))
                    {
                        commandeViewModel.Commande.Emails.Add(_authentificationRepository.FindUserByEmailByRoleAndRegion("Chef de ventes",
                            1).Result.Email);
                        commandeViewModel.Commande.Emails.Add( _authentificationRepository.FindUserByEmailByRoleAndRegion("Responsable commercial",
                            1).Result.Email);
                        
                        commandeViewModel.Commande.IdStatut = Statuts.EnCoursDeTraitement;
                        commandeViewModel.Commande.CommandeStatuts.Add(new CommandeStatutModel
                        {
                            StatutId = Statuts.ValidationDeLoffreDePrixCV
                        });
                        commandeViewModel.Commande.CommandeStatuts.Add(new CommandeStatutModel
                        {
                            StatutId = Statuts.ValidationDeLoffreDePrixRC
                        });
                    }
                    else if (commandeViewModel.DetailCommandes.Any(det => tarifs[det.IdArticle] - (double)det.Montant  >= 1 && tarifs[det.IdArticle] - (double)det.Montant  <=5))
                    {  var email = _authentificationRepository.FindUserByEmailByRoleAndRegion("Chef de ventes",
                            1).Result.Email;                        
                        commandeViewModel.Commande.Emails.Add(email);
                        commandeViewModel.Commande.IdStatut = Statuts.EnCoursDeTraitement;
                        commandeViewModel.Commande.CommandeStatuts.Add(new CommandeStatutModel
                        {
                            StatutId = Statuts.ValidationDeLoffreDePrixCV
                        });
                    }
                
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
                // Distinct emails en doublons
                commandeViewModel.Commande.Emails = commandeViewModel.Commande.Emails.Distinct().ToList();
                // Distinct status en doublons
                var status = commandeViewModel.Commande.CommandeStatuts.GroupBy(x => x.StatutId, (key,list)
                    => new CommandeStatutModel
                    {
                        StatutId = key
                     
                    }).ToList();
                // Add new Status
                commandeViewModel.Commande.CommandeStatuts.Clear();
                commandeViewModel.Commande.CommandeStatuts   = status;
                var ListStatusOffre = commandeViewModel.Commande.CommandeStatuts.Select(item => new OffreDePrix_Statut() { StatutId = item.StatutId, }).ToList();

                // Add commande
                commandeViewModel.Commande.IdClient = clientId;
                commandeViewModel.Commande.Currency = "MAD";
                commandeViewModel.Commande.IdChantier = ctnId;
                commandeViewModel.Commande.DateCommande = DateTime.UtcNow;
                //commandeViewModel.Commande.IsProspection = true;
                var Mt = commandeViewModel.DetailCommandes.Select(c => c.Volume * c.Montant).ToList();
                commandeViewModel.Commande.MontantCommande = Mt.Sum();

                var commande = _mapper.Map<CommandeModel, Commande>(commandeViewModel.Commande);              
                var commandeId = await _commandeRepository.CreateCommande(commande);

                //Add Offre de Prix
                var offreDePrix = new OffreDePrix
                {
                    ProspectId = prospectId,
                    Currency = commandeViewModel.Commande.Currency,
                    DateOffre = DateTime.Now,
                    MontantOffre = commandeViewModel.Commande.MontantCommande,
                    IdStatut = commandeViewModel.Commande.IdStatut,
                    LongFlech_Id = commandeViewModel.Commande.LongFleche_Id,
                    TarifAchatPompage = commandeViewModel.Commande.TarifAchatPompage,
                    TarifAchatTransport = commandeViewModel.Commande.TarifAchatTransport,
                    TarifVenteTransport = commandeViewModel.Commande.TarifVenteTransport,
                    TarifVentePompage = commandeViewModel.Commande.TarifVentePompage,
                    Conditions = commandeViewModel.Commande.Conditions,
                    Delai_Paiement = commandeViewModel.Commande.Delai_Paiement,
                    Commentaire = commandeViewModel.Commande.Commentaire,
                    ArticleFile = commandeViewModel.Commande.ArticleFile,
                    OffreStatuts = ListStatusOffre
                    //ArticleDescription = commandeViewModel.Commande.ArticleDescription
                };
                var offreid = await _commandeRepository.CreateOffreDePrix(offreDePrix);
                
                // Distinct articles en doublons
                var details = commandeViewModel.DetailCommandes.GroupBy(x => x.IdArticle, (key,list) => {
                    return new DetailCommandeModel
                    {
                        IdArticle = key,
                        Montant = list.FirstOrDefault().Montant,
                        Volume = list.Sum(x => x.Volume),
                        MontantRef = list.FirstOrDefault().MontantRef
                    };
                }).ToList();

                // Add details
                var listDetailsOffre = new List<OffreDePrix_Details>();
                foreach (var detail in details)
                {
                    detail.IdCommande = commandeId;
                    detail.Unite_Id = 1;
                    var detailOffre = new OffreDePrix_Details
                    {
                        IdOffre = offreid,
                        MontantRef = detail.MontantRef,
                        ArticleFile = detail.ArticleFile,
                        IdArticle = detail.IdArticle,
                        CodeArticleSap = detail.CodeArticleSap,
                        Montant = detail.Montant,
                        Volume = detail.Volume,
                        Unite_Id = detail.Unite_Id
                    };
                    listDetailsOffre.Add(detailOffre);

                }
               
                var detailCommandes = _mapper.Map<List<DetailCommandeModel>, List<DetailCommande>>(details);
                await _commandeRepository.CreateDetailCommande(detailCommandes);
                await _commandeRepository.CreateDetailOffreDePrix(listDetailsOffre);

                await transaction.CommitAsync();
                return commandeViewModel.Commande.Emails;
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                return null;
            }
        }

        public async Task<List<CommandeApiModel>> GetListProspects()
        {
            var prosepcts = await _commandeRepository.GetListProspects();
            return prosepcts.Select(item => new CommandeApiModel
                {
                    CommandeId = item.IdProspect,
                    CodeCommandeSap = item.CodeClientSap,
                    DateCommande = item.DateProspect,
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
                })
                .ToList();
        }

        public async Task<bool> CreateCommandeProspection(CommandeViewModel commandeViewModel)
        {
            await using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                if (commandeViewModel.ProspectionID is not null)
                {
                    var prospection = await _commandeRepository.GetCommande(commandeViewModel.ProspectionID);
                    // Add commande
                    commandeViewModel.CommandeV.IdClient = prospection.IdClient;
                    commandeViewModel.CommandeV.IdChantier = prospection.IdChantier;
                    commandeViewModel.CommandeV.CodeClientSap = prospection.CodeClientSap;
                    commandeViewModel.CommandeV.MontantCommande = prospection.MontantCommande;
                    commandeViewModel.CommandeV.TarifAchatTransport = prospection.TarifAchatTransport;
                    commandeViewModel.CommandeV.TarifAchatPompage = prospection.TarifAchatPompage;
                    commandeViewModel.CommandeV.TarifVentePompage = prospection.TarifVentePompage;
                    commandeViewModel.CommandeV.TarifVenteTransport = prospection.TarifVenteTransport;
                    commandeViewModel.CommandeV.Conditions = prospection.Conditions;
                    commandeViewModel.CommandeV.Delai_Paiement = prospection.Delai_Paiement;
                    commandeViewModel.CommandeV.LongFleche_Id = prospection.LongFleche_Id;
                    //commandeViewModel.CommandeV.Commentaire = prospection.Commentaire;
                    commandeViewModel.CommandeV.ArticleFile = prospection.ArticleFile;
                    commandeViewModel.CommandeV.ProspectionId = prospection.IdCommande;
                    
                }
                else
                {
                    // Add chantier
                    var chantier = _mapper.Map<ChantierModel, Chantier>(commandeViewModel.Chantier);
                    var ctnId = await _commandeRepository.CreateChantier(chantier);

                    // Add client
                    commandeViewModel.Client.Client_Ctn_Id = (int)ctnId;
                    var result = _commandeRepository.FindFormulaireClient(commandeViewModel.Client.Ice,
                        commandeViewModel.Client.Cnie, commandeViewModel.Client.RaisonSociale, null);
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
                    // Add commande
                    commandeViewModel.CommandeV.IdClient = clientId;
                    commandeViewModel.CommandeV.IdChantier = ctnId;
                    
                }

                commandeViewModel.CommandeV.IdStatut = Statuts.CommandeCree;
                commandeViewModel.CommandeV.Currency = "MAD";
                commandeViewModel.CommandeV.DateCommande = DateTime.Now;
                //commandeViewModel.Commande.IsProspection = true;
                var Mt = commandeViewModel.DetailCommandes.Select(c => c.Volume * c.Montant).ToList();
                commandeViewModel.CommandeV.MontantCommande = Mt.Sum();
                var commande = _mapper.Map<CommandeVModel, CommandeV>(commandeViewModel.CommandeV);              
                var commandeId = await _commandeRepository.CreateCommandeV(commande);
                /*if (commandeViewModel.CommandeV.DetailCommandes.Any(p => p.IdArticle == null))
                {
                    foreach (var item in commandeViewModel.CommandeV.DetailCommandes)
                    {
                        var article = _commandeRepository.GetArticleByDesi(item.)
                    }
                }*/
                // Distinct articles en doublons
                var details = commandeViewModel.DetailCommandes.GroupBy(x => x.IdArticle, (key,list) => {
                    return new DetailCommandeVModel
                    {
                        IdArticle = key,
                        Montant = list.FirstOrDefault().Montant,
                        Volume = list.Sum(x => x.Volume),
                        MontantRef = list.FirstOrDefault().MontantRef
                    };
                }).ToList();

                // Add details
                foreach (var detail in details)
                {
                    if (detail.IdArticle is null)
                    {
                        var article = await _commandeRepository.GetDetailCommande(detail.IdArticle);
                        //var unite = await  _commandeRepository.GetUniteByName(detail.)
                        detail.IdArticle = article.IdArticle;
                        detail.IdDetailCommande = 0;
                    }
                    detail.IdCommande = commandeId;
                    detail.Unite_Id = 1;
                }
                var detailCommandes = _mapper.Map<List<DetailCommandeVModel>, List<DetailCommandeV>>(details);
                await _commandeRepository.CreateDetailCommandeV(detailCommandes);

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

        public async Task<List<CommandeApiModel>> GetCommandes(List<int> ClientId, DateTime? DateCommande, string dateDebutSearch, string dateFinSearch)
        {
            var commandes =
                await _commandeRepository.GetCommandes(ClientId, DateCommande, dateDebutSearch, dateFinSearch);
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
        public async Task<CommandeVModel> GetCommandeV(int? id)
        {
            return _mapper.Map<CommandeVModel>(await _commandeRepository.GetCommandeV(id));
        }

        public async Task<List<TarifPompeRefModel>> GetTarifPompeRefs()
        {
            return _mapper.Map<List<TarifPompeRef>, List<TarifPompeRefModel>>(await this._commandeRepository.GetTarifPompeRefs());
        }
        public async Task<List<DetailCommandeApiModel>> GetCommandesDetails(int? commandeId)
        {
            var commandes = await _commandeRepository.GetListDetailsCommande(commandeId);
            if (commandes.Any())
                return commandes.Select(item => new DetailCommandeApiModel
                    {
                        IdDetailCommande = item.IdDetailCommande,
                        IdCommande = (int)item.IdCommande!,
                        ArticleDesignation = item.Article.Designation,
                        Montant = item.Montant,
                        DateProduction = item.DateProduction,
                        Volume = item.Volume,
                        UniteLibelle = item.Unite.Libelle,
                        ArticleFile = item.ArticleFile,
                        MontantRef = item.MontantRef,
                        IdArticle = item.IdArticle
                    })
                    .ToList();
            {
                var commandesV = await _commandeRepository.GetListDetailsCommandeV(commandeId);
                return commandesV.Select(item => new DetailCommandeApiModel
                    {
                        IdDetailCommande = item.IdDetailCommande,
                        IdCommande = (int)item.IdCommande!,
                        ArticleDesignation = item.Article.Designation,
                        Montant = item.Montant,
                        DateProduction = item.DateProduction,
                        Volume = item.Volume,
                        UniteLibelle = item.Unite.Libelle,
                        ArticleFile = item.ArticleFile,
                        MontantRef = item.MontantRef,
                        IdArticle = item.IdArticle
                    })
                    .ToList();
            }
            //return mapper.Map<List<DetailCommande>, List<DetailCommandeModel>>(commandes);
            
        }

        public async Task<List<ValidationEtatModel>> GetCommandesStatuts(int? id) =>
        _mapper.Map<List<ValidationEtat>,List<ValidationEtatModel>>(await _commandeRepository.GetCommandesStatuts(id));
        public async Task<bool> ProposerPrix(int Id, decimal Tarif, string UserName, string articleFile, decimal tarifAchat)
        {
            await using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var detail = await _commandeRepository.GetDetailCommande(Id);
                var commande = await _commandeRepository.GetCommande(detail.IdCommande);
                var user = await _authentificationRepository.FindUserByEmail(UserName);
                var userRole = await _authentificationRepository.GetUserRole(user);
                decimal marge = ((Tarif - tarifAchat) / tarifAchat) * 100;

                //Update Detail + Commande
                detail.MontantV1 = Tarif;
                detail.MargeBenef = marge;
                detail.ArticleFile = articleFile;
                detail.Commande.MontantCommande += detail.Montant;

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
                
                var listValidateurs = await _commandeRepository.GetListValidation(commande.IdCommande);
                
                switch (marge)
                {
                    case < 25 and >= 20:
                    {
                        await _commandeRepository.AddStatutCommande( new CommandeStatut
                        {
                            CommandeId = commande.IdCommande,
                            StatutId = Statuts.ValidationDeLoffreDePrixRC
                        });
                        break;
                    }
                    case < 20:
                        await _commandeRepository.AddStatutCommande(new CommandeStatut
                        {
                            CommandeId = commande.IdCommande,
                            StatutId = Statuts.ValidationDeLoffreDePrixRC
                        });
                        await _commandeRepository.AddStatutCommande( new CommandeStatut
                        {
                            CommandeId = commande.IdCommande,
                            StatutId = Statuts.ValidationDeLoffreDePrixDABPE
                        });
                        break;
                }
                if (listValidateurs.Any() && listValidateurs.Count == commande.CommandeStatuts.Count)
                    commande.IdStatut = Statuts.Validé;
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

        public async Task<string> FixationPrixTransport(int Id, double VenteT, double VenteP, string email)
        {
            try
            {
                var commande = await _commandeRepository.GetCommandeOnly(Id);
                var user = await _authentificationRepository.FindUserByEmail(email);
                var region = user.VilleId;
                var userRole = await _authentificationRepository.GetUserRole(user);
                commande.MontantCommande += (decimal?)(commande.TarifAchatTransport - VenteT);
                commande.TarifAchatTransport = VenteT;
                var commercialEmail = _authentificationRepository.FindUserByEmailByRoleAndRegion("Commercial", region).Result.Email;

                // Trace Vlidateur
                var validationModel = new ValidationModel
                {
                    IdCommande = commande.IdCommande,
                    IdStatut = Statuts.FixationDePrixDuTransport,
                    Date = DateTime.Now,
                    UserId = user.Id,
                    Nom = user.Nom,
                    Prenom = user.Prenom,
                    Fonction = userRole,
                    ValidationLibelle = "Fixation de prix du transport"
                };
                var validation = _mapper.Map<ValidationModel, Validation>(validationModel);
                await _commandeRepository.CreateValidation(validation);
                
                var listValidateurs = await _commandeRepository.GetListValidation(commande.IdCommande);
                if (listValidateurs.Any() && listValidateurs.Count == commande.CommandeStatuts.Count)
                    commande.IdStatut = Statuts.Validé;
                await _unitOfWork.Complete();
                return commercialEmail;
            }
            catch(Exception ex)
            {
                return string.Empty;
            }
        }

        public ClientModel FindFormulaireClient(string ice, string cnie, string Rs, int? IdClient)
        {
            var result =  _commandeRepository.FindFormulaireClient(ice, cnie, Rs, IdClient);
            return _mapper.Map<Client,ClientModel>(result);
        }  
        public ChantierModel FindFormulaireChantier(int? IdChantier)
        {
            var result =  _commandeRepository.FindFormulaireChantier(IdChantier);
            return _mapper.Map<Chantier,ChantierModel>(result);
        }

        public async Task<List<ValidationModel>> GetListValidation(int commandeId) =>
            _mapper.Map<List<Validation>,List<ValidationModel>>(await _commandeRepository.GetListValidation(commandeId));

        public async Task<bool> SetCommande(int commandeId) => await _commandeRepository.SetCommande(commandeId);
        public async Task<List<CommandeApiModel>> GetCommandesValide(List<int> clientId, DateTime? dateTime, string dateDebutSearch, string dateFinSearch)
        { 
            var commandes = await _commandeRepository.GetCommandesValide(clientId, dateTime,dateDebutSearch,dateFinSearch);
            return commandes.Select(item => new CommandeApiModel
                {
                    CommandeId = item.IdCommande,
                    CodeCommandeSap = item.CodeClientSap,
                    StatutCommande = item.Statut.Libelle,
                    DateCommande = item.DateCommande,
                    HeureLivraisonSouhaite = item.DateLivraisonSouhaite,
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
                    Pays = item.Client.Pays.NomPays,
                    PresenceLabo = item.PresenceLabo
                    // DetailsCommande  = listDetailCommandeApi
                })
                .ToList();        
        }
        
        public async Task<bool> FixationPrixRC(List<CommandeModifVenteApi> commandeModifApi, string UserEmail, int IdCommande)
        {
            await using var transaction = _unitOfWork.BeginTransaction();
            var isSpecial = false;
            try
            {
                var commande = await _commandeRepository.GetCommande(IdCommande);
                var user = await _authentificationRepository.FindUserByEmail(UserEmail);
                var userRole = await _authentificationRepository.GetUserRole(user);
                foreach (var item in commandeModifApi)
                {
                    var detail = await _commandeRepository.GetDetailCommande(item.idDetailCommande);
                   // if (detail.IdArticle == 14)
                       // isSpecial = true;
                    detail.Montant = item.montant;
                  //  detail.ArticleFile = item.CommandeBetonArticleFile;
                    detail.Commande.MontantCommande += detail.Montant;
                }
                // detail.Commande.IdStatut = Statuts.ValidationDeLoffreDePrix;

                // Trace Vlidateur
                var validationModel = new ValidationModel
                {
                    IdCommande = IdCommande,
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
                if (isSpecial && userRole == "Responsable commercial")
                {

                    commande.IdStatut = Statuts.EnCoursDeTraitement;
                    commande.CommandeStatuts.Add(new CommandeStatut
                    {
                        StatutId = Statuts.ValidationDeLoffreDePrixDABPE

                    });
                }
                else
                {
                    var listValidateurs = await _commandeRepository.GetListValidation(commande.IdCommande);
                
                    if (listValidateurs.Any() && listValidateurs.Count == commande.CommandeStatuts.Count)
                        commande.IdStatut = Statuts.Validé;
                }
              
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
    }
}
