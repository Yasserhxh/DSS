using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Nom",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Prenom",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    Article_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designation = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Tarif = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.Article_Id);
                });

            migrationBuilder.CreateTable(
                name: "Delai_Paiement",
                columns: table => new
                {
                    Delai_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Delai_Libelle = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delai_Paiement", x => x.Delai_Id);
                });

            migrationBuilder.CreateTable(
                name: "Forme_Juridique",
                columns: table => new
                {
                    FormeJuridique_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormeJuridique_Libelle = table.Column<string>(type: "nvarchar(150)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forme_Juridique", x => x.FormeJuridique_Id);
                });

            migrationBuilder.CreateTable(
                name: "Paiement",
                columns: table => new
                {
                    Paiement_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Conditions = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paiement", x => x.Paiement_Id);
                });

            migrationBuilder.CreateTable(
                name: "Pays",
                columns: table => new
                {
                    IdPays = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodePaysSap = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    NomPays = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pays", x => x.IdPays);
                });

            migrationBuilder.CreateTable(
                name: "Statut",
                columns: table => new
                {
                    IdStatut = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeStatutSap = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Libelle = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statut", x => x.IdStatut);
                });

            migrationBuilder.CreateTable(
                name: "Tarif_Pompe",
                columns: table => new
                {
                    Tpr_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LongFleche_Libelle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LongFleche_Prix = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarif_Pompe", x => x.Tpr_Id);
                });

            migrationBuilder.CreateTable(
                name: "Tarif_Ref",
                columns: table => new
                {
                    Tbr_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designation_Beton = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tarif = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarif_Ref", x => x.Tbr_Id);
                });

            migrationBuilder.CreateTable(
                name: "Tarif_Service",
                columns: table => new
                {
                    TS_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TarifTransport = table.Column<double>(type: "float", nullable: false),
                    TarifPompage = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarif_Service", x => x.TS_Id);
                });

            migrationBuilder.CreateTable(
                name: "Type_Chantier",
                columns: table => new
                {
                    Tc_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tc_Libelle = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type_Chantier", x => x.Tc_Id);
                });

            migrationBuilder.CreateTable(
                name: "Unite",
                columns: table => new
                {
                    IdUnite = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libelle = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unite", x => x.IdUnite);
                });

            migrationBuilder.CreateTable(
                name: "Zone",
                columns: table => new
                {
                    Zone_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Zone_Libelle = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Zone_Prix = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zone", x => x.Zone_Id);
                });

            migrationBuilder.CreateTable(
                name: "Ville",
                columns: table => new
                {
                    IdVille = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeVilleSap = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    NomVille = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    IdPays = table.Column<int>(type: "int", nullable: true),
                    CodePaysSap = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ville", x => x.IdVille);
                    table.ForeignKey(
                        name: "FK_Ville_Pays_IdPays",
                        column: x => x.IdPays,
                        principalTable: "Pays",
                        principalColumn: "IdPays");
                });

            migrationBuilder.CreateTable(
                name: "Centrale_Beton",
                columns: table => new
                {
                    Ctr_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ctr_Nom = table.Column<string>(type: "nvarchar(80)", nullable: true),
                    Ctr_Adresse = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Ctr_CodePostal = table.Column<int>(type: "int", nullable: true),
                    Ctr_Ville_Id = table.Column<int>(type: "int", nullable: true),
                    Ctr_Gsm = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Ctr_Email = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Ctr_Responsable = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Ctr_Responsable_Gsm = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Ctr_Latiture = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    Ctr_Longitude = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    Rayon = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Centrale_Beton", x => x.Ctr_Id);
                    table.ForeignKey(
                        name: "FK_Centrale_Beton_Ville_Ctr_Ville_Id",
                        column: x => x.Ctr_Ville_Id,
                        principalTable: "Ville",
                        principalColumn: "IdVille");
                });

            migrationBuilder.CreateTable(
                name: "Chantier",
                columns: table => new
                {
                    Ctn_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ctn_Nom = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    MaitreOuvrage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VolumePrevisonnel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Duree = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rayon = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Ctn_Zone_Id = table.Column<int>(type: "int", nullable: false),
                    Ctn_Tc_Id = table.Column<int>(type: "int", nullable: false),
                    Ctn_Ctr_Id = table.Column<int>(type: "int", nullable: false),
                    Ctn_Adresse = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Ctn_Latiture = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    Ctn_Longitude = table.Column<string>(type: "nvarchar(30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chantier", x => x.Ctn_Id);
                    table.ForeignKey(
                        name: "FK_Chantier_Centrale_Beton_Ctn_Ctr_Id",
                        column: x => x.Ctn_Ctr_Id,
                        principalTable: "Centrale_Beton",
                        principalColumn: "Ctr_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chantier_Type_Chantier_Ctn_Tc_Id",
                        column: x => x.Ctn_Tc_Id,
                        principalTable: "Type_Chantier",
                        principalColumn: "Tc_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chantier_Zone_Ctn_Zone_Id",
                        column: x => x.Ctn_Zone_Id,
                        principalTable: "Zone",
                        principalColumn: "Zone_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Client_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeClientSap = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    FormeJuridique_Id = table.Column<int>(type: "int", nullable: false),
                    Ice = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Cnie = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Gsm = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    RaisonSociale = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Adresse = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    Destinataire_Interlocuteur = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    IdVille = table.Column<int>(type: "int", nullable: true),
                    IdPays = table.Column<int>(type: "int", nullable: true),
                    Client_Ctn_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Client_Id);
                    table.ForeignKey(
                        name: "FK_Client_Chantier_Client_Ctn_Id",
                        column: x => x.Client_Ctn_Id,
                        principalTable: "Chantier",
                        principalColumn: "Ctn_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Client_Forme_Juridique_FormeJuridique_Id",
                        column: x => x.FormeJuridique_Id,
                        principalTable: "Forme_Juridique",
                        principalColumn: "FormeJuridique_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Client_Pays_IdPays",
                        column: x => x.IdPays,
                        principalTable: "Pays",
                        principalColumn: "IdPays");
                    table.ForeignKey(
                        name: "FK_Client_Ville_IdVille",
                        column: x => x.IdVille,
                        principalTable: "Ville",
                        principalColumn: "IdVille");
                });

            migrationBuilder.CreateTable(
                name: "Commande",
                columns: table => new
                {
                    IdCommande = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeCommandeSap = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    IdClient = table.Column<int>(type: "int", nullable: true),
                    IdChantier = table.Column<int>(type: "int", nullable: true),
                    CodeClientSap = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCommande = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    MontantCommande = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DateLivraisonSouhaite = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    IdStatut = table.Column<int>(type: "int", nullable: true),
                    LongFleche_Id = table.Column<int>(type: "int", nullable: true),
                    TarifAchatTransport = table.Column<double>(type: "float", nullable: false),
                    TarifAchatPompage = table.Column<double>(type: "float", nullable: false),
                    TarifVenteTransport = table.Column<double>(type: "float", nullable: false),
                    TarifVentePompage = table.Column<double>(type: "float", nullable: false),
                    Conditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Delai_Paiement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Commentaire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArticleFile = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commande", x => x.IdCommande);
                    table.ForeignKey(
                        name: "FK_Commande_Chantier_IdChantier",
                        column: x => x.IdChantier,
                        principalTable: "Chantier",
                        principalColumn: "Ctn_Id");
                    table.ForeignKey(
                        name: "FK_Commande_Client_IdClient",
                        column: x => x.IdClient,
                        principalTable: "Client",
                        principalColumn: "Client_Id");
                    table.ForeignKey(
                        name: "FK_Commande_Statut_IdStatut",
                        column: x => x.IdStatut,
                        principalTable: "Statut",
                        principalColumn: "IdStatut");
                    table.ForeignKey(
                        name: "FK_Commande_Tarif_Pompe_LongFleche_Id",
                        column: x => x.LongFleche_Id,
                        principalTable: "Tarif_Pompe",
                        principalColumn: "Tpr_Id");
                });

            migrationBuilder.CreateTable(
                name: "DetailCommande",
                columns: table => new
                {
                    IdDetailCommande = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCommande = table.Column<int>(type: "int", nullable: true),
                    CodeCommandeSap = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    IdArticle = table.Column<int>(type: "int", nullable: true),
                    CodeArticleSap = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Montant = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Volume = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DateProduction = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Unite_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailCommande", x => x.IdDetailCommande);
                    table.ForeignKey(
                        name: "FK_DetailCommande_Article_IdArticle",
                        column: x => x.IdArticle,
                        principalTable: "Article",
                        principalColumn: "Article_Id");
                    table.ForeignKey(
                        name: "FK_DetailCommande_Commande_IdCommande",
                        column: x => x.IdCommande,
                        principalTable: "Commande",
                        principalColumn: "IdCommande");
                    table.ForeignKey(
                        name: "FK_DetailCommande_Unite_Unite_Id",
                        column: x => x.Unite_Id,
                        principalTable: "Unite",
                        principalColumn: "IdUnite",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Validateur",
                columns: table => new
                {
                    Validation_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ValidationLibelle = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Nom = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Prenom = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Fonction = table.Column<string>(type: "nvarchar(80)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCommande = table.Column<int>(type: "int", nullable: false),
                    IdStatut = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Validateur", x => x.Validation_Id);
                    table.ForeignKey(
                        name: "FK_Validateur_Commande_IdCommande",
                        column: x => x.IdCommande,
                        principalTable: "Commande",
                        principalColumn: "IdCommande",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Centrale_Beton_Ctr_Ville_Id",
                table: "Centrale_Beton",
                column: "Ctr_Ville_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Chantier_Ctn_Ctr_Id",
                table: "Chantier",
                column: "Ctn_Ctr_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Chantier_Ctn_Tc_Id",
                table: "Chantier",
                column: "Ctn_Tc_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Chantier_Ctn_Zone_Id",
                table: "Chantier",
                column: "Ctn_Zone_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Client_Client_Ctn_Id",
                table: "Client",
                column: "Client_Ctn_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Client_FormeJuridique_Id",
                table: "Client",
                column: "FormeJuridique_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Client_IdPays",
                table: "Client",
                column: "IdPays");

            migrationBuilder.CreateIndex(
                name: "IX_Client_IdVille",
                table: "Client",
                column: "IdVille");

            migrationBuilder.CreateIndex(
                name: "IX_Commande_IdChantier",
                table: "Commande",
                column: "IdChantier");

            migrationBuilder.CreateIndex(
                name: "IX_Commande_IdClient",
                table: "Commande",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Commande_IdStatut",
                table: "Commande",
                column: "IdStatut");

            migrationBuilder.CreateIndex(
                name: "IX_Commande_LongFleche_Id",
                table: "Commande",
                column: "LongFleche_Id");

            migrationBuilder.CreateIndex(
                name: "IX_DetailCommande_IdArticle",
                table: "DetailCommande",
                column: "IdArticle");

            migrationBuilder.CreateIndex(
                name: "IX_DetailCommande_IdCommande",
                table: "DetailCommande",
                column: "IdCommande");

            migrationBuilder.CreateIndex(
                name: "IX_DetailCommande_Unite_Id",
                table: "DetailCommande",
                column: "Unite_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Validateur_IdCommande",
                table: "Validateur",
                column: "IdCommande");

            migrationBuilder.CreateIndex(
                name: "IX_Ville_IdPays",
                table: "Ville",
                column: "IdPays");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Delai_Paiement");

            migrationBuilder.DropTable(
                name: "DetailCommande");

            migrationBuilder.DropTable(
                name: "Paiement");

            migrationBuilder.DropTable(
                name: "Tarif_Ref");

            migrationBuilder.DropTable(
                name: "Tarif_Service");

            migrationBuilder.DropTable(
                name: "Validateur");

            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropTable(
                name: "Unite");

            migrationBuilder.DropTable(
                name: "Commande");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Statut");

            migrationBuilder.DropTable(
                name: "Tarif_Pompe");

            migrationBuilder.DropTable(
                name: "Chantier");

            migrationBuilder.DropTable(
                name: "Forme_Juridique");

            migrationBuilder.DropTable(
                name: "Centrale_Beton");

            migrationBuilder.DropTable(
                name: "Type_Chantier");

            migrationBuilder.DropTable(
                name: "Zone");

            migrationBuilder.DropTable(
                name: "Ville");

            migrationBuilder.DropTable(
                name: "Pays");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nom",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Prenom",
                table: "AspNetUsers");
        }
    }
}
