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
            CreateMap<Commande, CommandeModel>();
            CreateMap<DetailCommande, DetailCommandeModel>();
            CreateMap<CentraleBeton, CentraleBetonModel>();
            CreateMap<FormeJuridique, FormeJuridiqueModel>();
            CreateMap<TypeChantier, TypeChantierModel>();
            CreateMap<Zone, ZoneModel>();
            CreateMap<Article, ArticleModel>();
            CreateMap<TarifPompeRef,TarifPompeRefModel>();


            // Models to Entities mapping
            CreateMap<UserModel, ApplicationUser>();
            CreateMap<UpdateUserModel, ApplicationUser>();
            CreateMap<RegisterModel, ApplicationUser>();
            CreateMap<ClientModel, Client>();
            CreateMap<ChantierModel, Chantier>();
            CreateMap<CommandeModel, Commande>();
            CreateMap<DetailCommandeModel, DetailCommande>();
            CreateMap<CentraleBetonModel, CentraleBeton>();
            CreateMap<FormeJuridiqueModel, FormeJuridique>();
            CreateMap<TypeChantierModel, TypeChantier>();
            CreateMap<ZoneModel, Zone>();
            CreateMap<ArticleModel, Article>();
            CreateMap<TarifPompeRefModel, TarifPompeRef>();

        }
    }
}
