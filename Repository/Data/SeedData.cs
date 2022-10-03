using System.Collections.Generic;
using Domain.Authentication;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Repository.Data
{
    public class SeedData
    {
        public List<User> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public List<IdentityUserRole<string>> UserRoles { get; set; }
        public List<Statut> Statuts { get; set; }
        public List<Unite> Unites { get; set; }
        public List<Pays> Pays { get; set; }
        public List<Ville> Villes { get; set; }
        public List<FormeJuridique> FormeJuridiques { get; set; }
        public List<Zone> Zones { get; set; }
        public List<TypeChantier> TypeChantiers { get; set; }
        public List<CentraleBeton> CentraleBetons { get; set; }
        public List<Article> Articles { get; set; }
        public List<TarifPompeRef> TarifPompeRefs { get; set; }
    }

        public class User : ApplicationUser
        {
            public string Password { get; set; }
        }
    
}