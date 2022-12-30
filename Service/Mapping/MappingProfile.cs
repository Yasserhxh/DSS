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
            CreateMap<Client, ClientModel>();
            CreateMap<Chantier, ChantierModel>();
            CreateMap<OffreDePrix, OffreDePrixModel>();
            CreateMap<CommandeFinale, CommandeFinaleModel>();
            CreateMap<Prospect, ProspectModel>();
            CreateMap<OffreDePrix_Details, OffreDePrix_DetailsModel>();
            CreateMap<CommandeFinale_Details, CommandeFinale_DetailsModel>();
            CreateMap<Commande, CommandeModel>()
                .ForMember(c => c.Emails, opt => opt.Ignore());
            CreateMap<CommandeV, CommandeVModel>();
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
            CreateMap<ClientModel, Client>();
            CreateMap<ChantierModel, Chantier>();
            CreateMap<CommandeModel, Commande>();
            CreateMap<CommandeVModel, CommandeV>();
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
