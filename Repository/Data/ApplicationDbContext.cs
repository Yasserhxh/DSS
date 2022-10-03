using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Authentication;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

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
            SeedData(builder, this);
        }
        private static void SeedData(ModelBuilder builder, ApplicationDbContext applicationDbContext)
        {
            var content = File.ReadAllText(Path.Combine("../Repository/Data/seedData.json"));
            //var content = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Data/seedData.json"));
            var seedData = JsonConvert.DeserializeObject<SeedData>(content);
            var hasher = new PasswordHasher<ApplicationUser>();
            builder.Entity<IdentityRole>().HasData(seedData.Roles.ToArray());
            builder.Entity<ApplicationUser>().HasData(seedData.Users.Select(user => new ApplicationUser()
            {
                Id = user.Id,
                SecurityStamp = user.SecurityStamp,
                UserName = user.UserName,
                PasswordHash = hasher.HashPassword(user, user.Password),
                NormalizedUserName = user.NormalizedUserName,
                Email = user.Email,
                NormalizedEmail = user.Email?.ToUpper(),
                EmailConfirmed = user.EmailConfirmed,
                Nom = user.Nom,
                Prenom = user.Prenom
            }).ToArray());
            builder.Entity<IdentityUserRole<string>>().HasData(seedData.UserRoles.ToArray());
            builder.Entity<Statut>().HasData(seedData.Statuts.ToArray());
            builder.Entity<Unite>().HasData(seedData.Unites.ToArray());
            builder.Entity<Pays>().HasData(seedData.Pays.ToArray());
            builder.Entity<Ville>().HasData(seedData.Villes.ToArray());
            builder.Entity<FormeJuridique>().HasData(seedData.FormeJuridiques.ToArray());
            builder.Entity<Zone>().HasData(seedData.Zones.ToArray());
            builder.Entity<TypeChantier>().HasData(seedData.TypeChantiers.ToArray());
            builder.Entity<CentraleBeton>().HasData(seedData.CentraleBetons.ToArray());
            builder.Entity<Article>().HasData(seedData.Articles.ToArray());
            builder.Entity<TarifPompeRef>().HasData(seedData.TarifPompeRefs.ToArray());
        }
    }
}
