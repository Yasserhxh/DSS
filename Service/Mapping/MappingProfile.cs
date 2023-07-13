using AutoMapper;
using Domain.Authentication;
using Domain.Entities;
using Domain.Models;

namespace Service.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entities to Models mapping
            CreateMap<ApplicationUser, UserModel>();
            CreateMap<ApplicationUser, RegisterModel>();
            CreateMap<ApplicationUser, UpdateUserModel>();

            CreateMap<Client, ClientModel>()
                .ForMember(c => c.Adresse, act => act.MapFrom(src => src.Adresse.ToUpper()))
                .ForMember(c => c.Cnie, act => act.MapFrom(src => src.Cnie!.ToUpper()))
                .ForMember(c => c.Ice, act => act.MapFrom(src => src.Ice!.ToUpper()))
                .ForMember(c => c.Destinataire_Interlocuteur, act => act.MapFrom(src => src.Destinataire_Interlocuteur!.ToUpper()))
                .ForMember(c => c.RaisonSociale, act => act.MapFrom(src => src.RaisonSociale!.ToUpper()));

            CreateMap<Chantier, ChantierModel>()
                .ForMember(c => c.MaitreOuvrage, act => act.MapFrom(src => src.MaitreOuvrage.ToUpper()))
                .ForMember(c => c.Ctn_Adresse, act => act.MapFrom(src => src.Ctn_Adresse.ToUpper()))
                .ForMember(c => c.Ctn_Nom, act => act.MapFrom(src => src.Ctn_Nom.ToUpper()));

            CreateMap<OffreDePrix, OffreDePrixModel>();
            CreateMap<CommandeFinale, CommandeFinaleModel>();
            CreateMap<Prospect, ProspectModel>();
            CreateMap<OffreDePrix_Details, OffreDePrix_DetailsModel>();
            CreateMap<CommandeFinale_Details, CommandeFinale_DetailsModel>();

            CreateMap<Commande, CommandeModel>()
                .ForMember(c => c.Emails, opt => opt.Ignore())
                .ForMember(c => c.Delai_Paiement, act => act.MapFrom(src => src.Delai_Paiement.ToUpper()))
                .ForMember(c => c.Conditions, act => act.MapFrom(src => src.Conditions.ToUpper()));

            CreateMap<CommandeV, CommandeVModel>()
                .ForMember(c => c.Delai_Paiement, act => act.MapFrom(src => src.Delai_Paiement.ToUpper()))
                .ForMember(c => c.Conditions, act => act.MapFrom(src => src.Conditions.ToUpper()));

            CreateMap<DetailCommande, DetailCommandeModel>();
            CreateMap<DetailCommandeV, DetailCommandeVModel>();
            CreateMap<CentraleBeton, CentraleBetonModel>();
            CreateMap<FormeJuridique, FormeJuridiqueModel>();
            CreateMap<TypeChantier, TypeChantierModel>();
            CreateMap<Zone, ZoneModel>();
            CreateMap<Article, ArticleModel>();
            CreateMap<TarifPompeRef,TarifPompeRefModel>();
            CreateMap<Statut,StatutModel>();
            CreateMap<DelaiPaiement,DelaiPaiementModel>();
            CreateMap<Pays,PaysModel>();
            CreateMap<Ville,VilleModel>();
            CreateMap<CommandeStatut,CommandeStatutModel>();
            CreateMap<Validation,ValidationModel>();
            CreateMap<ValidationEtat,ValidationEtatModel>();
            CreateMap<OffreDePrix_Statut,OffreDePrix_StatutModel >();

            // Models to Entities mapping
            CreateMap<UserModel, ApplicationUser>();
            CreateMap<UpdateUserModel, ApplicationUser>();
            CreateMap<RegisterModel, ApplicationUser>();
            CreateMap<ClientModel, Client>()
                .ForMember(c => c.Adresse, act => act.MapFrom(src => src.Adresse.ToUpper()))
                .ForMember(c => c.Cnie, act => act.MapFrom(src => src.Cnie!.ToUpper()))
                .ForMember(c => c.Ice, act => act.MapFrom(src => src.Ice!.ToUpper()))
                .ForMember(c => c.Destinataire_Interlocuteur, act => act.MapFrom(src => src.Destinataire_Interlocuteur!.ToUpper()))
                .ForMember(c => c.RaisonSociale, act => act.MapFrom(src => src.RaisonSociale!.ToUpper()));



            CreateMap<ChantierModel, Chantier>()
                .ForMember(c => c.MaitreOuvrage, act => act.MapFrom(src => src.MaitreOuvrage.ToUpper()))
                .ForMember(c => c.Ctn_Adresse, act => act.MapFrom(src => src.Ctn_Adresse.ToUpper()))
                .ForMember(c => c.Ctn_Nom, act => act.MapFrom(src => src.Ctn_Nom.ToUpper()));

            CreateMap<CommandeModel, Commande>()
                .ForMember(c => c.Delai_Paiement, act => act.MapFrom(src => src.Delai_Paiement.ToUpper()))
                .ForMember(c => c.Conditions, act => act.MapFrom(src => src.Conditions.ToUpper()));

            CreateMap<CommandeVModel, CommandeV>()
                .ForMember(c => c.Delai_Paiement, act => act.MapFrom(src => src.Delai_Paiement.ToUpper()))
                .ForMember(c => c.Conditions, act => act.MapFrom(src => src.Conditions.ToUpper()));

            CreateMap<DetailCommandeModel, DetailCommande>();
            CreateMap<DetailCommandeVModel, DetailCommandeV>();
            CreateMap<CentraleBetonModel, CentraleBeton>();
            CreateMap<FormeJuridiqueModel, FormeJuridique>();
            CreateMap<TypeChantierModel, TypeChantier>();
            CreateMap<ZoneModel, Zone>();
            CreateMap<ArticleModel, Article>();
            CreateMap<TarifPompeRefModel, TarifPompeRef>();
            CreateMap<StatutModel,Statut>();
            CreateMap<DelaiPaiementModel,DelaiPaiement>();
            CreateMap<PaysModel,Pays>();
            CreateMap<VilleModel,Ville>();
            CreateMap<CommandeStatutModel,CommandeStatut>();
            CreateMap<ValidationModel,Validation>();
            CreateMap<ValidationEtatModel,ValidationEtat>();
            CreateMap<OffreDePrixModel, OffreDePrix>();
            CreateMap<CommandeFinaleModel,CommandeFinale >();
            CreateMap<ProspectModel,Prospect >();
            CreateMap<OffreDePrix_DetailsModel,OffreDePrix_Details >();
            CreateMap<CommandeFinale_DetailsModel,CommandeFinale_Details >();
            CreateMap<OffreDePrix_StatutModel,OffreDePrix_Statut >();
        }
    }
}
