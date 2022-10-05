using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Authentication;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Reflection;

namespace Repository.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<CentraleBeton> CentraleBetons { get; set; }
        public DbSet<Chantier> Chantiers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<DelaiPaiement> DélaiPaiements { get; set; }
        public DbSet<FormeJuridique> FormeJuridiques { get; set; }
        public DbSet<Paiement> Paiements { get; set; }
        public DbSet<Pays> Pays { get; set; }
        public DbSet<TarifBetonRef> TarifBetonRefs { get; set; }
        public DbSet<TarifPompeRef> TarifPompeRefs { get; set; }
        public DbSet<TarifService> TarifServices { get; set; }
        public DbSet<TypeChantier> TypeChantiers { get; set; }
        public DbSet<Ville> Villes { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<DetailCommande> DetailCommandes { get; set; }
        public DbSet<Statut> Statuts { get; set; }
        public DbSet<Unite> Unites { get; set; }
        public DbSet<Validation> Validations {  get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //SeedData(builder, this);
        }
        
    }
}
